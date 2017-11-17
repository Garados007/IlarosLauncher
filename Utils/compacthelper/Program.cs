using System;
using System.IO;
using MaxLib.Data.CompactFileSystem;

namespace compacthelper
{
    class Program
    {
        static string source, target;
        static bool delete, create, blockwise, singlefile, extended;
        static long pointer = 3, block = 65535;

        static FileStream fs;
        static CompactSystem compact;

        static int Main(string[] args)
        {
            if (!parseArgs(args))
            {
                Console.WriteLine(Properties.Resources.ConsoleHelp);
                return 0;
            }
            if (!Directory.Exists(source))
            {
                Console.Error.WriteLine("Quellordner existiert nicht");
                return 1;
            }
            if (!create && !File.Exists(target))
            {
                Console.Error.WriteLine("Zieldatei existiert nicht. Erstellen Sie eine mit dem Parameter /N.");
                return 1;
            }
            if (!(File.Exists(target) ? OpenTarget() : CreateTarget())) return 2;
            Console.WriteLine("Übetrage Dateien und Ordner ...");
            ReadDirectory(new DirectoryInfo(source), null);
            Console.WriteLine("Übertragen abgeschlossen");
            if (delete)
            {
                Console.WriteLine("Lösche Quellordner ...");
                Directory.Delete(source, true);
                Console.WriteLine("Löschen abgeschlossen");
            }
            Console.WriteLine("Speichere Daten");
            compact.Flush();
            compact.Dispose();
            fs.Dispose();
            Console.WriteLine("Abgeschlossen");
            return 0;
        }



        static void ReadDirectory(DirectoryInfo di, CompactEntry entry)
        {
            foreach (var sd in di.GetDirectories())
            {
                var child = entry == null ? compact.FileTable.GetRootEntry(di.Name) : entry.GetChild(di.Name);
                if (child == null)
                    child = compact.FileTable.CreateEntry(di.Name, CompactEntryFlags.DirectoryMode, entry);
                ReadDirectory(sd, child);
            }
            foreach (var fi in di.GetFiles())
            {
                ReadFile(fi, entry);
            }
        }

        static void ReadFile(FileInfo fi, CompactEntry parent)
        {
            var entry = parent == null ? compact.FileTable.GetRootEntry(fi.Name) : parent.GetChild(fi.Name);
            if (entry == null)
                entry = compact.FileTable.CreateEntry(fi.Name, CompactEntryFlags.None, parent);
            using (var ss = fi.OpenRead())
            using (var ts = entry.GetContent())
            {
                ss.Position = ts.Position = 0;
                var buffer = new byte[65536];
                int readed;
                while ((readed = ss.Read(buffer, 0, buffer.Length)) != 0)
                    ts.Write(buffer, 0, readed);
                ts.SetLength(ts.Position);
            }
        }

        static bool CreateTarget()
        {
            CompactPointerSize ps;
            switch (pointer)
            {
                case 1: ps = CompactPointerSize.Byte1; break;
                case 2: ps = CompactPointerSize.Byte2; break;
                case 3: ps = CompactPointerSize.Byte3; break;
                case 4: ps = CompactPointerSize.Byte4; break;
                case 5: ps = CompactPointerSize.Byte5; break;
                case 6: ps = CompactPointerSize.Byte6; break;
                case 7: ps = CompactPointerSize.Byte7; break;
                default: ps = CompactPointerSize.Byte8; break;
            }
            CompactSystemFlags flags = CompactSystemFlags.None;
            if (blockwise) flags |= CompactSystemFlags.ReferenceByBlock;
            if (singlefile) flags |= CompactSystemFlags.SingleFileMode;
            if (extended) flags |= CompactSystemFlags.ExtendedAttribute;
            
            try { fs = new FileStream(target, FileMode.OpenOrCreate, FileAccess.ReadWrite); }
            catch
            {
                Console.Error.WriteLine("Zieldatei konnte nicht erzeugt werden.");
                return false;
            }
            try { compact = new CompactSystem(fs, ps, flags, (ulong)block); }
            catch
            {
                Console.Error.WriteLine("Zielsystem konnte nicht erzeugt werden.");
                fs.Dispose();
                File.Delete(target);
                return false;
            }
            return true;
        }

        static bool OpenTarget()
        {
            try { fs = new FileStream(target, FileMode.OpenOrCreate, FileAccess.ReadWrite); }
            catch
            {
                Console.Error.WriteLine("Zieldatei geöffnet werden.");
                return false;
            }
            try { compact = new CompactSystem(fs); }
            catch
            {
                Console.Error.WriteLine("Zielsystem konnte nicht eingelesen werden.");
                fs.Dispose();
                return false;
            }
            return true;
        }

        static bool parseArgs(string[] args)
        {
            bool sourceset = false, targetset = false;
            for (int i = 0; i<args.Length; ++i)
            {
                if (args[i].StartsWith("/") || args[i].StartsWith("-"))
                {
                    bool nextValid = i < args.Length - 1 && !args[i + 1].StartsWith("/") 
                        && !args[i + 1].StartsWith("-");
                    switch (args[i].Substring(1).ToLower())
                    {
                        case "s": if (!nextValid) return false;
                            source = args[i + 1]; i++; sourceset = true; break;
                        case "t": if (!nextValid) return false;
                            target = args[i + 1]; i++; targetset = true; break;
                        case "d": delete = true; break;
                        case "n": create = true; break;
                        case "p": if (!nextValid || !long.TryParse(args[i + 1], out pointer)) return false;
                            if (pointer < 1 || pointer > 8) return false;
                            i++; break;
                        case "b": if (!nextValid || !long.TryParse(args[i + 1], out block)) return false;
                            if (block < 1) return false;
                            i++; break;
                        case "r": blockwise = true; break;
                        case "sf": singlefile = true; break;
                        case "e": extended = true; break;
                        default: return false;
                    }
                }
                else return false;
            }
            return sourceset && targetset;
        }
    }
}
