using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Renci.SshNet;

class HostInfo
{
    public string hostname;
    public string address;
    public int port;
    public string username;
    public string keyFile;
}

class MainClass
{
    public static void Main (string [] args)
    {
        string version = GetVersion ();
        Console.WriteLine ($"SFTP upload utility v{version}");

        // this should be taken from command line agrguments
        string filePath = "/home/user/rep.txt";
        string targetPath = "/var/www/" + Path.GetFileName (filePath);
        string hostname = "hostsailor";

        string homeDirectory = GetHomeDirectory ();
        string configPath = homeDirectory + ".ssh/config";
        HostInfo [] hosts = ReadOpenSSHConfig (configPath);
        HostInfo hi = Array.Find (hosts, x => { return string.Compare (x.hostname, hostname) == 0; });
        if (hi == null) {
            Console.WriteLine ($"HostName {hostname} is not found in configuration file {configPath}");
            return;
        }

        // this should be parsed from openssh config
        // /home/user/.ssh/config
        PrivateKeyFile [] keyfiles;
        using (FileStream fs = new FileStream (hi.keyFile, FileMode.Open)) {
            keyfiles = new PrivateKeyFile [1] { new PrivateKeyFile (fs) };
        };

        using (SftpClient client = new SftpClient (hi.address, hi.port, hi.username, keyfiles)) {
            client.Connect ();
            using (FileStream fs = new FileStream (filePath, FileMode.Open)) {
                client.UploadFile (fs, targetPath);
            }
        }

    }

    static string GetHomeDirectory ()
    {
        // https://stackoverflow.com/questions/1143706/getting-the-path-of-the-home-directory-in-c
        string homeDirectory = Environment.GetFolderPath (Environment.SpecialFolder.UserProfile);
        //Console.WriteLine ($"Home directory: {homeDirectory}");
        return homeDirectory;
    }

    static string GetVersion ()
    {
        Assembly assembly = Assembly.GetExecutingAssembly ();
        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo (assembly.Location);
        string version = fileVersionInfo.ProductVersion;
        return version;
    }

    static HostInfo [] ReadOpenSSHConfig (string configPath)
    {
        HostInfo hi = new HostInfo ();
        hi.hostname = "hostsailor";
        hi.address = "185.106.120.6";
        hi.port = 60271;
        hi.username = "ruler";
        hi.keyFile = "/home/user/.ssh/ruler.key";
        HostInfo [] res = new HostInfo [1] { hi };
        return res;
    }
}
