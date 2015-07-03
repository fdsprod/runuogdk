using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;

namespace Ultima.GDK
{
    public delegate void HotKeyEventHandler(object sender, HotKeyEventArgs args);

    public struct KeyData
    {
        public KeyData(bool control, bool shift, bool alt, Keys key)
        {
            Control = control;
            Shift = shift;
            Alt = alt;
            Key = key;
        }

        public bool Control;
        public bool Shift;
        public bool Alt;
        public Keys Key;
    }

    public class HotKey
    {
        private string name;
        private KeyData keys;

        public string Name { get { return name; } }
        public KeyData Keys { get { return keys; } }
        
        public HotKeyEventHandler HotKeyDelegate;

        public HotKey(string name, KeyData keys)
        {
            this.name = name;
            this.keys = keys;
        }
    }

    public class HotKeyManager
    {
        private List<string> assemblies;
        private List<string> namespaces;
        private Dictionary<MethodInfo,Assembly> assemblyTable;
        private Dictionary<KeyData, MethodInfo> hotKeyTable;

        public HotKeyManager(string file)
        {
            assemblies = new List<string>();
            namespaces = new List<string>();

            assemblyTable = new Dictionary<MethodInfo, Assembly>();
            hotKeyTable = new Dictionary<KeyData, MethodInfo>();

            if (File.Exists(file))
            {
                LoadHotKeys(file);
            }
        }

        private void LoadHotKeys(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlDocumentType docType = doc.CreateDocumentType("GDKML", null, null, null);
            string dtdString = docType.InnerXml;

            XmlElement root = doc["Hotkeys"];

            int count = root.ChildNodes.Count;

            for (int c = 0; c < count; c++)
            {
                XmlElement node = (XmlElement)root.ChildNodes[c];

                if (node.Name == "References")
                {
                    int referenceCount = node.ChildNodes.Count;

                    for (int a = 0; a < referenceCount; a++)
                    {
                        XmlElement child = (XmlElement)node.ChildNodes[a];

                        if (child.Name == "Assembly")
                        {
                            string assembly = Utility.GetAttributeString(child, "file");

                            if (String.IsNullOrEmpty(assembly))
                            {
                                MessageBox.Show("Reference number " + a.ToString() + " does not contain a file name and will not be loaded.", "RunUO: GDK");
                                continue;
                            }

                            assemblies.Add(assembly);
                        }
                        else if (child.Name == "Namespace")
                        {
                            string nmspace = Utility.GetAttributeString(child, "name");

                            if (String.IsNullOrEmpty(nmspace))
                            {
                                MessageBox.Show("Reference number " + a.ToString() + " does not contain a file name and will not be loaded.", "RunUO: GDK");
                                continue;
                            }

                            namespaces.Add(nmspace);
                        }
                    }
                }
                else if (node.Name == "Hotkey" && Utility.GetAttributeBoolean(node, "compile", true))
                {
                    int hotKeyCount = node.ChildNodes.Count;
                    string name = Utility.GetAttributeString(node, "name");

                    if(String.IsNullOrEmpty(name))
                    {
                        MessageBox.Show("A Hotkey does not contain a name and will not be loaded", "RunUO: GDK");
                        continue;
                    }

                    KeyData keyData = new KeyData();

                    keyData.Control = Utility.GetAttributeBoolean(node, "control", false);
                    keyData.Alt = Utility.GetAttributeBoolean(node, "alt", false);
                    keyData.Shift = Utility.GetAttributeBoolean(node, "shift", false);
                    keyData.Key = Utility.GetAttributeKey(node, "key");

                    if (keyData.Key == Keys.None)
                    {
                        MessageBox.Show("Hotkey " + name + " does not have a key specified and will not be loaded.", "RunUO: GDK");
                        continue;
                    }

                    StringBuilder cb = new StringBuilder();

                    for (int a = 0; a < namespaces.Count; a++)
                    {
                        cb.AppendLine("using " + namespaces[a] + ";");
                    }

                    string ctl = keyData.Control.ToString().ToLower();
                    string shf = keyData.Shift.ToString().ToLower();
                    string alt = keyData.Alt.ToString().ToLower();
                    string key = Enum.GetName(typeof(Keys), keyData.Key);

                    cb.AppendLine("");
                    cb.AppendLine("namespace Ultima.GDK {");
                    cb.AppendLine("\tpublic class " + name + "Hotkey {");
                    cb.AppendLine("\t\tpublic " + name + "Hotkey() { }");
                    cb.AppendLine("\t\tpublic KeyData KeyData { get { return new KeyData(" + ctl + ", " + shf + ", " + alt + ", " + "Keys." + key + "); } }");
                    cb.AppendLine("\t\tpublic void Invoke" + "(object sender, HotKeyEventArgs args) { ");
                    cb.AppendLine("\t\t\tif(args == null) {");
                    cb.AppendLine("\t\t\t\tthrow new Exception(\"Delegate arguments are null\");");
                    cb.AppendLine("\t\t\t}");
                    cb.AppendLine("");
                    cb.AppendLine("\t\t\tGump Gump = args.Gump;");
                    cb.AppendLine("\t\t\tDesignerFrame Designer = args.Designer;");
                    cb.AppendLine("");
                    cb.AppendLine("\t\t\t" + node.InnerText.Trim());
                    cb.AppendLine("\t\t}");
                    cb.AppendLine("\t}");
                    cb.AppendLine("}");

                    CSharpCodeProvider csp = new CSharpCodeProvider();
                    ICodeCompiler cc = csp.CreateCompiler();
                    CompilerParameters cp = new CompilerParameters();

                    cp.WarningLevel = 3;

                    cp.ReferencedAssemblies.Add("mscorlib.dll");
                    cp.ReferencedAssemblies.Add("System.dll");
                    cp.ReferencedAssemblies.Add("System.Xml.dll");
                    cp.ReferencedAssemblies.Add("System.Data.dll");
                    cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                    cp.ReferencedAssemblies.Add("Ultima.dll");
                    cp.ReferencedAssemblies.Add("Ultima.GDK.dll");
                    cp.ReferencedAssemblies.Add("RunUO GDK.exe");

                    for (int a = 0; a < assemblies.Count; a++)
                    {
                        cp.ReferencedAssemblies.Add(assemblies[a]);
                    }

                    cp.CompilerOptions = "/target:library /optimize";
                    cp.GenerateExecutable = false;
                    cp.GenerateInMemory = false;

                    string outputAssembly = Application.StartupPath + "\\HotkeyAssemblies\\" + name + "Hotkey.dll";

                    if(!Directory.Exists(Path.GetDirectoryName(outputAssembly)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(outputAssembly));
                    }

                    cp.OutputAssembly = outputAssembly;

                    System.CodeDom.Compiler.TempFileCollection tfc = new TempFileCollection(Application.StartupPath, false);
                    CompilerResults cr = new CompilerResults(tfc);

                    cr = cc.CompileAssemblyFromSource(cp, cb.ToString());

                    if (cr.Errors.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("An error occured while compiling Hotkey: " + name + "\n");

                        for (int b = 0; b < cr.Errors.Count; b++)
                        {
                            sb.AppendLine(cr.Errors[b].ToString());
                        }

                        MessageBox.Show(sb.ToString(), "RunUO: GDK");
                        continue;
                    }                    
                }
            }

            LoadAssemblies(Application.StartupPath + "\\HotkeyAssemblies\\");
        }

        public void LoadAssemblies(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(folder);
            FileInfo[] files = dir.GetFiles("*.dll");

            for (int i = 0; i < files.Length; i++)
            {
                Assembly asm = Assembly.LoadFile(files[i].FullName);
                Type[] asmTypes = asm.GetTypes();

                if (asmTypes.Length > 0)
                {
                    Type type = asmTypes[0];
                    object obj = Activator.CreateInstance(type);

                    MethodInfo deleg = type.GetMethod("Invoke");
                    PropertyInfo prop = type.GetProperty("KeyData");

                    if (deleg == null || prop == null )
                    {
                        MessageBox.Show("Assembly:\n\n" + asm.FullName + "\n\nHotkey will not be loaded", "RunUO: GDK");
                        continue;
                    }

                    KeyData keyData = (KeyData)prop.GetValue(obj, null);

                    assemblyTable.Add(deleg, asm);
                    Register(keyData, deleg);
                }       
            }
        }

        private void Register(KeyData keys, MethodInfo method)
        {
            if (hotKeyTable.ContainsKey(keys))
            {
                MessageBox.Show("Duplicate hotkey detected, hotkey #" + hotKeyTable.Count.ToString() + " was not registered.", "RunUO: GDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hotKeyTable.Add(keys, method);
        }

        public void OnKeyDown(object sender, HotKeyEventArgs args)
        {

            IEnumerator<KeyData> enumorator = hotKeyTable.Keys.GetEnumerator();

            while (enumorator.MoveNext())
            {
                Console.WriteLine("Key: {0}, Shift: {1}", args.KeyData.Key, args.KeyData.Shift);

                KeyData data = enumorator.Current;
                if (Utility.IsKeyDown(data.Key) && data.Alt == args.KeyData.Alt &&
                    data.Control == args.KeyData.Control && data.Shift == args.KeyData.Shift)
                {
                    if (hotKeyTable.ContainsKey(data))
                    {
                        MethodInfo handler = hotKeyTable[data];

                        if (handler != null)
                        {
                            Type type = assemblyTable[handler].GetTypes()[0];
                            object obj = Activator.CreateInstance(type);
                            handler.Invoke(obj, new object[] { sender, args });
                        }
                    }
                }
            }
        }
    }
}
