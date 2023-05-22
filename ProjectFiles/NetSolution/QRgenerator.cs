#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Alarm;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.NativeUI;
using FTOptix.Recipe;
using FTOptix.DataLogger;
using FTOptix.EventLogger;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
using FTOptix.EthernetIP;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using Net.Codecrete.QrCodeGenerator;
using System.IO;
using System.Text;
#endregion

public class QRgenerator : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void CreateCode(string code, string fileName)
    {
        var qr = QrCode.EncodeText(code, QrCode.Ecc.Medium);
        string svg = qr.ToSvgString(4);
        string filePath = ResourceUri.FromApplicationRelativePath("").Uri + "/" + fileName;
        File.WriteAllText(filePath, svg, Encoding.UTF8);
        ((Image)Owner).Path = filePath;
    }
}
