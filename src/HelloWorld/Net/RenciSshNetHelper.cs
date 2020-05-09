/*
 * GitHub :https://github.com/sshnet/SSH.NET
 * 
 * SSH��
 * Secure Shell����д��ͨ��ʹ��SSH�����԰����д�������ݽ��м��ܣ������ܹ���ֹDNS��ƭ��IP��ƭ��ʹ��SSH�������Խ����������ѹ�������Կ��Լӿ촫����ٶȡ�SSH����ΪFTP�ṩһ����ȫ�ġ�ͨ������
 * ������Ӧ�ò�ʹ��������ϵİ�ȫЭ�飬����Ҫ��������������ɣ���ͬʵ��SSH�İ�ȫ���ܻ��ơ�
        �����Э�飬���ṩ������֤�����κ������Լ���Ȱ�ȫ��ʩ��������������������ṩ����ѹ�����ܡ�ͨ������£���Щ�����Э�鶼�������������ӵ�TCP������֮�ϡ�
        �û���֤Э��㣬����ʵ�ַ������ĸ��ͻ����û�֮��������֤���������ڴ����Э��֮�ϡ�
        ����Э��㣬����������ͨ����һЩ�߼�ͨ���ϣ����������û���֤��Э��֮�ϡ�
   ����ȫ�Ĵ�������ӽ���֮�󣬿ͻ��˽�����һ���������󡣵��û���֤�����ӽ���֮�󽫷��͵ڶ�������������������¶����Э����Ժ���ǰ��Э�鹲�档����Э���ṩ����������Ŀ��ͨ����Ϊ���ð�ȫ����Shell�Ự�ʹ��������TCP/IP�˿ں�X11�����ṩ��׼������
   
   SSH�ṩ���ּ���İ�ȫ��֤��SSH1��SSH2��
    SSH1�����ڿ���İ�ȫ��֤����ֻҪ��֪���Լ����ʺźͿ���Ϳ��Ե�¼��Զ���������������д�������ݶ��ᱻ���ܡ����ǣ�������֤��ʽ���ܱ�֤���������ӵķ����������������ӵķ����������ܻ��б�ķ�������ð�������ķ�������Ҳ�����ܵ�"�м���"���ֹ�����ʽ�Ĺ�����
    SSH2�������ܳ׵İ�ȫ��֤������Ҫ�����ܳף�Ҳ���������Ϊ�Լ�����һ���ܳף����ѹ����ܳ׷�����Ҫ���ʵķ������ϡ������Ҫ���ӵ�SSH�������ϣ��ͻ�������� ���������������������������ܳ׽��а�ȫ��֤���������յ�����֮���������ڸ÷��������û���Ŀ¼��Ѱ����Ĺ����ܳף�Ȼ��������㷢�͹����Ĺ����� �׽��бȽϡ���������ܳ�һ�£����������ù����ܳ׼���"��ѯ"��challenge�����������͸��ͻ���������ͻ�������յ�"��ѯ"֮��Ϳ�������� ˽���ܳ׽����ٰ������͸���������

    �Ƚϣ�SSH1��ȣ�SSH2����Ҫ�������ϴ����û�������⣬SSH2�����������д��͵����ݣ���"�м���"���ֹ�����ʽҲ�ǲ����ܵģ���Ϊ��û�����˽���ܳף�������������¼�Ĺ��̿�����һЩ��

SSH�����Ӧ�þ��ǣ�������ȡ����ͳ��Telnet��FTP������Ӧ�ó���ͨ��SSH��¼��Զ������ִ��������еĹ���������ڲ���ȫ����·ͨѶ�����У����ṩ�˺�ǿ����֤��authentication��������ǳ���ȫ��ͨѶ������
 * SFTP��ʹ��SSHЭ�����FTP�����Э�飬��ȫ�ļ�����Э��
 * 
 * SFTP����ˣ�
 * Windows ��װSFTP
 * 1.����freesshd ��http://www.freesshd.com/?ctt=download
 * 2. freeSSHd.exe
 * 3.��װ
 * 4.
 * 
 * 
 * SFTP�ͻ��ˣ�
 * Filezillia
 * https://filezilla-project.org/download.php?type=client
 * 
 * �ο����ϣ�
 * https://www.cnblogs.com/happyday56/p/5664693.html
 * 
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Net;
using Renci.SshNet;

 namespace HelloWorld.IPC
{
    public class RenciSshNetHelper
    {
        /// <summary>
        /// SSH1 - �û���������֤
        /// ʹ������͹�Կ��֤����SFTP����
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="psw"></param>
        /// <param name="keyName"></param>
        /// <param name="client"></param>
        public static void EstablishConnection(string host, string userName, string name, string psw, string keyName, out SftpClient client)
        {
            var connectionInfo = new ConnectionInfo("sftp.foo.com",
                                        "guest",
                                        new PasswordAuthenticationMethod("guest", "pwd"),
                                        new PrivateKeyAuthenticationMethod("rsa.key"));
            client = new SftpClient(connectionInfo);
            client.Connect();
        }

        /// <summary>
        /// SSH2 - ��Կ˽Կ��֤
        /// ʹ���û��������뽨��SSH���ӣ�����֤�����ָ��
        /// </summary>
        public static void VerifyHostIdentify()
        {
            byte[] expectedFingerPrint = new byte[] {
                                            0x66, 0x31, 0xaf, 0x00, 0x54, 0xb9, 0x87, 0x31,
                                            0xff, 0x58, 0x1c, 0x31, 0xb1, 0xa2, 0x4c, 0x6b
                                        };
            using (var client = new SshClient("sftp.foo.com", "guest", "pwd"))
            {
                client.HostKeyReceived += (sender, e) =>
                {
                    if (expectedFingerPrint.Length == e.FingerPrint.Length)
                    {
                        for (var i = 0; i < expectedFingerPrint.Length; i++)
                        {
                            if (expectedFingerPrint[i] != e.FingerPrint[i])
                            {
                                e.CanTrust = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        e.CanTrust = false;
                    }
                };
                client.Connect();
            }
        }
    }
}