using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SSHClient
{
    public class StartContainerResult
    {
        public string containerId { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {


            if (args.Length != 4)
            {
                Console.WriteLine("Usage: SSHClient host[:port] user password runCommand");
                Console.WriteLine("Example: SSHClient 123.456.789.012:50000 root mypass123 \"ps -all\"");
                return;
            }

            int port = 22;
            string host = args[0];
            string user = args[1];
            string password = args[2];
            string runCommand = args[3];
            string[] hostAndPort = host.Split(':');
            if (hostAndPort.Length == 2)
            {
                host = hostAndPort[0];
                port = int.Parse(hostAndPort[1]);
            }

            //var kpgen = new RsaKeyPairGenerator();
            //kpgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 2048));
            //var keyPair = kpgen.GenerateKeyPair();
            //PrivateKeyInfo pkInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
            //String privateKey = Convert.ToBase64String(pkInfo.GetDerEncoded());
            //SubjectPublicKeyInfo info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);
            //String publicKey = Convert.ToBase64String(info.GetDerEncoded());

            //runCommand = "sudo adduser user2 --gecos \"First Last, RoomNumber, WorkPhone, HomePhone\" --disabled-password && echo \"user2:newpassword\" | sudo chpasswd";
            // runCommand = "sudo mkdir /home/user2/.ssh; sudo chmod 700  /home/user2/.ssh; sudo echo \"ssh-rsa " + publicKey + "\" >> /home/user2/.ssh/authorized_keys";


            //string sshPrivateKeyString =
            //    "-----BEGIN RSA PRIVATE KEY-----\n"+
            //    "jqGjEBs9ssaz8e6zGd2FvPFLY8dnWhmhrAVMxRnniSv8mPN4Z+kGnZzm9ykwSG1a\n" +
            //    "TUCcTO2101j4ma9jwrqn2Ik1S+fBaFkU+mLz3CljGs2DccTAOd232gEcojcPeLgT\n" +
            //    "52N1Az1oYgrBRebtNuEtsFOYbyJJIA+xINqH+rzW4bxkyhwk8RuMhr0g3DWvHyKu\n" +
            //    "Pjd/CS3uPDq43PtrFpbnvTWkQnVCDu240CGhxPPjbXntBhGB5pB1o6KUPTZcDZTL\n" +
            //    "9UkWJ76AO+z6SXwqP75jwRykNWsE1lnvUN/QKM55Ibc00aBstIhdefMFOOpHPtRN\n" +
            //    "by2Xpe0k6RSVG/Wy9NsJjxeLVt99xpoqmv/Jy9EXkSMI+2Cq60xSzDGS0ruLNQaS\n" +
            //    "Ka3KrsyePLnrSCsQ0xJ7JqxHLg04GGzhVP7tSr2mTe/Ks57lhu0yTHIAUY8ISQ0j\n" +
            //    "ZOOUYLG+8GZXOIyefzaZEwmhLRIOdO6xwTfgpyYa7ObGsWhjbUyGAwBWFyC332Qv\n" +
            //    "TryBknzAIKUHNj/cG9tEuWsop5+yNo8BPkXWag8MzoyaeqsKQXgkDdKHsabp5Im2\n" +
            //    "3j/Nf+fel01/MEDKbQdLQBd2euuNh3AUohjkYe/o43nIeMbyBObSTyuCKMp+0QR8\n" +
            //    "HMsGahKEcXgGnio7YqJ2dAHprd9YvOD2qtsZ0sFUZK4PqyWm9e3E2VjANKayv3Im\n" +
            //    "Y6WtxS8kLg1dvQCQ/vSPxN9Axk1gE+3Qt1r0f6ZvFzc+JOf4sHeAXXzoclI5waMw\n" +
            //    "Vnojmpbhm1WEySA4c6rHuO2/jErij9Wh1ccH+FDu6SkjrC1t0F2BPdOpKNLvTd7b\n" +
            //    "rfRQLEJNnnfg7XQZeqY7m5NzA+BA40fyS2zgZCXMTup3akMRcDQVJfdg9l6nHuwu\n" +
            //    "-----END RSA PRIVATE KEY-----";


            // var pk = new PrivateKeyFile(new MemoryStream(Encoding.ASCII.GetBytes(sshPrivateKeyString)));
            //var pk = new PrivateKeyFile(@"C:\Users\atanask\Documents\a.txt", "730717");

            //ConnectionInfo ci = new PrivateKeyConnectionInfo(host, port, user, pk);

            string response = ExecuteSSHCommand(port, host, user, password, GetContainerStartCommand("80:80", "trescst/container-info"));
            StartContainerResult r = JsonConvert.DeserializeObject<StartContainerResult>(response);

            ExecuteSSHCommand(port, host, user, password, GetContainerStopCommand(r.containerId));

        }

        private static string ExecuteSSHCommand(int port, string host, string user, string password, string runCommand)
        {
            string result = string.Empty;
            DateTime startTime = DateTime.Now;
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(host, port, user, password);
            using (var client = new SshClient(connectionInfo))
            {
                Console.WriteLine("Connecting to: {0}@{1}:{2}", user, host, port);
                client.Connect();

                Console.WriteLine("Executing command:\n{0}", runCommand);
                SshCommand command = client.RunCommand(runCommand);
                result = command.Result;
                Console.WriteLine("\nOutput:\n{0}", command.Result);
                if (!string.IsNullOrEmpty(command.Error))
                {
                    Console.WriteLine("\nError: \n{0}", command.Error);
                }
                Console.WriteLine("\nExit code: {0}", command.ExitStatus);

                client.Disconnect();
            }

            TimeSpan ts = DateTime.Now - startTime;
            Console.WriteLine("Execution Time: {0} seconds", ts.TotalSeconds);
            return result;
        }

        public static string GetContainerStartCommand(string ports, string imageName)
        {
            // Executing the command should return Result in json {id:containerId}
            return string.Format("containerId=$(sudo docker run -d --restart=always -p {0} {1}); echo {{containerId:\"'$containerId'\"}}", ports, imageName);
        }
        public static string GetContainerStopCommand(string containerid)
        {
            return string.Format("sudo docker stop {0}; sudo docker rm {0}", containerid);
        }
    }
}
