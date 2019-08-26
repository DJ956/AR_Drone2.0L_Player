using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace AR_Drone2._0L_Player.ControllerCommand
{
    [Serializable]
    public class ControllerSetting
    {
        private static readonly string FOLDER_NAME = "ArDrone2.0";
        private static readonly string FILE_NAME = "Controller.xml";

        private static readonly string ROOT =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FOLDER_NAME);
        private static readonly string PATH =
            Path.Combine(ROOT, FILE_NAME);

        public float UpMax { get; set; }
        public float DownMax { get; set; }
        public float LeftMax { get; set; }
        public float RightMax { get; set; }
        public float ForwardMax { get; set; }
        public float BackMax { get; set; }
        public float TurnLeftMax { get; set; }
        public float TurnRightMax { get; set; }


        public ControllerSetting(float upMax, float downMax, float leftMax, float rightMax,
            float forwardMax, float backMax, float turnLeftMax, float turnRightMax)
        {
            UpMax = upMax;
            DownMax = downMax;
            LeftMax = leftMax;
            RightMax = rightMax;
            ForwardMax = forwardMax;
            BackMax = backMax;
            TurnLeftMax = turnLeftMax;
            TurnRightMax = turnRightMax;
        }

        public ControllerSetting() { }

        /// <summary>
        /// コントローラー設定値をXMLに保存する
        /// パスはドキュメントフォルダ内
        /// </summary>
        /// <param name="setting"></param>
        public static void Save(ControllerSetting setting)
        {
            if (!Directory.Exists(ROOT)) { Directory.CreateDirectory(ROOT); }

            var serializer = new XmlSerializer(typeof(ControllerSetting));
            using (var stream = new StreamWriter(PATH, false, Encoding.UTF8))
            {
                serializer.Serialize(stream, setting);
                stream.Flush();
            }
        }

        /// <summary>
        /// コントローラーの設定値ファイルを読み込む
        /// </summary>
        /// <returns></returns>
        public static ControllerSetting Load()
        {
            //ファイルが存在しなければ作成
            if (!File.Exists(PATH))
            {
                var setting = new ControllerSetting(0.5f, -0.5f, -0.1f, 0.1f, -0.25f, 0.25f, 0.5f, -0.5f);
                Save(setting);
            }

            var serializer = new XmlSerializer(typeof(ControllerSetting));
            using (var reader = new StreamReader(PATH, Encoding.UTF8))
            using (var xmlReader = XmlReader.Create(reader, new XmlReaderSettings() { CheckCharacters = false }))
            {
                var obj = serializer.Deserialize(xmlReader);
                return (ControllerSetting)obj;
            }
        }

    }
}
