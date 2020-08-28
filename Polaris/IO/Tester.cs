//  This file is part of Polaris-IO - An IO wrapper for Unity.
//  https://github.com/dynamiquel/Polaris-IO
//  Copyright (c) 2020 dynamiquel

//  MIT License
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using UnityEngine;
using CompressionType = Polaris.IO.Compression.CompressionType;

namespace Polaris.IO
{
    public class Tester : MonoBehaviour
    {
        private TestObject testObject = new TestObject();
        private string textExample;
        private string path;
        private Stopwatch sw = new Stopwatch();

        private void Start()
        {
            path = Path.Combine(Application.persistentDataPath, "IO Testing");

            testObject.PlayerName = "Polaris Warrior";
            testObject.ModsEnabled = false;
            testObject.ItemIDs.Add("goldToothbrush");
            testObject.ItemIDs.Add("snowWand");
            testObject.ItemIDs.Add("diamondRing");

            var sb = new StringBuilder();
            for (int i = 0; i < 500; i++)
                sb.AppendLine(
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.\nSed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?\nBut I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?\nAt vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.\nOn the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that pleasures have to be repudiated and annoyances accepted. The wise man therefore always holds in these matters to this principle of selection: he rejects pleasures to secure other greater pleasures, or else he endures pains to avoid worse pains.");

            textExample = sb.ToString();
            testObject.ItemIDs.Add(textExample);
        }

        #region Text

        private TestResult TextWrite()
        {
            sw.Restart();
            
            try
            {
                Text.Write(Path.Combine(path, "text"), textExample, CompressionType.None);
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                return new TestResult(false, sw.Elapsed);
            }
        }

        private TestResult TextRead()
        {
            sw.Restart();

            string content = Text.Read(Path.Combine(path, "text"), CompressionType.None);
            return new TestResult(content == textExample, sw.Elapsed);
        }

        private async Task<TestResult> TextWriteAsync()
        {
            sw.Restart();

            try
            {
                Text.WriteAsync(Path.Combine(path, "text_async"), textExample, CompressionType.None).GetAwaiter().GetResult();
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                return new TestResult(false, sw.Elapsed);
            }
        }

        private async Task<TestResult> TextReadAsync()
        {
            sw.Restart();

            string content = await Text.ReadAsync(Path.Combine(path, "text_async"), CompressionType.None);
            return new TestResult(content == textExample, sw.Elapsed);
        }

        #endregion

        #region Json

        private TestResult JsonWrite()
        {
            sw.Restart();
            
            try
            {
                Json.Write(Path.Combine(path, "json"), testObject, CompressionType.None);
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                throw e;
                return new TestResult(false, sw.Elapsed);
            }
        }

        private TestResult JsonRead()
        {
            sw.Restart();

            var content = Json.Read<TestObject>(Path.Combine(path, "json"), CompressionType.None);

            return new TestResult(content.Equals(testObject), sw.Elapsed);
        }

        private async Task<TestResult> JsonWriteAsync()
        {
            sw.Restart();

            try
            {
                await Json.WriteAsync(Path.Combine(path, "json_async"), testObject, CompressionType.Zip);
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                return new TestResult(false, sw.Elapsed);
            }
        }

        private async Task<TestResult> JsonReadAsync()
        {
            sw.Restart();

            var content = await Json.ReadAsync<TestObject>(Path.Combine(path, "json_async"), CompressionType.Zip);
            return new TestResult(content.Equals(testObject), sw.Elapsed);
        }

        #endregion

        #region Yaml

        private TestResult YamlWrite()
        {
            sw.Restart();
            
            try
            {
                Yaml.Write(Path.Combine(path, "yaml"), testObject);
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                return new TestResult(false, sw.Elapsed);
            }
        }

        private TestResult YamlRead()
        {
            sw.Restart();

            var content = Yaml.Read<TestObject>(Path.Combine(path, "yaml"));

            return new TestResult(content.Equals(testObject), sw.Elapsed);
        }

        private async Task<TestResult> YamlWriteAsync()
        {
            sw.Restart();

            try
            {
                await Yaml.WriteAsync(Path.Combine(path, "yaml_async"), testObject);
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                return new TestResult(false, sw.Elapsed);
            }
        }

        private async Task<TestResult> YamlReadAsync()
        {
            sw.Restart();

            var content = Yaml.ReadAsync<TestObject>(Path.Combine(path, "yaml_async")).GetAwaiter().GetResult();

            return new TestResult(content.Equals(testObject), sw.Elapsed);
        }

        #endregion

        #region Binary

        private TestResult BinaryWrite()
        {
            sw.Restart();

            try
            {
                Binary.Write(Path.Combine(path, "binary"), testObject, CompressionType.Lzma);
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                return new TestResult(false, sw.Elapsed);
            }
        }

        private TestResult BinaryRead()
        {
            sw.Restart();

            var content = Binary.Read<TestObject>(Path.Combine(path, "binary"), CompressionType.Lzma);

            return new TestResult(content.Equals(testObject), sw.Elapsed);
        }

        private async Task<TestResult> BinaryWriteAsync()
        {
            sw.Restart();

            try
            {
                await Binary.WriteAsync(Path.Combine(path, "binary_async"), testObject, CompressionType.Zip);
                return new TestResult(true, sw.Elapsed);
            }
            catch (Exception e)
            {
                return new TestResult(false, sw.Elapsed);
            }
        }

        private async Task<TestResult> BinaryReadAsync()
        {
            sw.Restart();

            var content = await Binary.ReadAsync<TestObject>(Path.Combine(path, "binary_async"), CompressionType.Zip);

            return new TestResult(content.Equals(testObject), sw.Elapsed);
        }

        #endregion

        #region Buttons

        public async void AutoTest()
        {
            var tests = new Dictionary<string, TestResult>
            {
                ["Text_Write"] = TextWrite(),
                ["Text_Read"] = TextRead(),
                ["Text_Write_Async"] = await TextWriteAsync(),
                ["Text_Read_Async"] = await TextReadAsync(),
                ["Json_Write"] = JsonWrite(),
                ["Json_Read"] = JsonRead(),
                ["Json_Write_Async"] = await JsonWriteAsync(),
                ["Json_Read_Async"] = await JsonReadAsync(),
                ["Yaml_Write"] = YamlWrite(),
                ["Yaml_Read"] = YamlRead(),
                ["Yaml_Write_Async"] = await YamlWriteAsync(),
                ["Yaml_Read_Async"] = await YamlReadAsync(),
                ["Binary_Write"] = BinaryWrite(),
                ["Binary_Read"] = BinaryRead(),
                ["Binary_Write_Async"] = await BinaryWriteAsync(),
                ["Binary_Read_Async"] = await BinaryReadAsync()
            };

            var passes = (byte) tests.Values.Count(result => result.success);

            var sb = new StringBuilder($"Polaris IO Tests [{passes}/{tests.Count} Passes]\n");

            foreach (var test in tests)
            {
                var result = test.Value.success ? "Pass" : "Fail";
                
                sb.AppendLine($"${test.Key} [{result}] - {test.Value.elapsedTime}");
            }

            UnityEngine.Debug.Log(sb.ToString());
        }

        public void TextWrite_Click()
        {
            var result = TextWrite();
            UnityEngine.Debug.Log($"Text_Write [{result.success}] - {result.elapsedTime}");
        }

        public void TextRead_Click()
        {
            var result = TextRead();
            UnityEngine.Debug.Log($"Text_Read[{result.success}] - {result.elapsedTime}");
        }

        public async void TextWriteAsync_Click()
        {
            var result = await TextWriteAsync();
            UnityEngine.Debug.Log($"Text_WriteAsync [{result.success}] - {result.elapsedTime}");
        }

        public async void TextReadAsync_Click()
        {
            var result = await TextReadAsync();
            UnityEngine.Debug.Log($"Text_ReadAsync [{result.success}] - {result.elapsedTime}");
        }

        public void JsonWrite_Click()
        {
            var result = JsonWrite();
            UnityEngine.Debug.Log($"Json_Write [{result.success}] - {result.elapsedTime}");
        }

        public void JsonRead_Click()
        {
            var result = JsonRead();
            UnityEngine.Debug.Log($"Json_Read [{result.success}] - {result.elapsedTime}");
        }

        public async void JsonWriteAsync_Click()
        {
            var result = await JsonWriteAsync();
            UnityEngine.Debug.Log($"Json_WriteAsync [{result.success}] - {result.elapsedTime}");
        }

        public async void JsonReadAsync_Click()
        {
            var result = await JsonReadAsync();
            UnityEngine.Debug.Log($"Json_ReadAsync [{result.success}] - {result.elapsedTime}");
        }

        public void YamlWrite_Click()
        {
            var result = YamlWrite();
            UnityEngine.Debug.Log($"Yaml_Write [{result.success}] - {result.elapsedTime}");
        }

        public void YamlRead_Click()
        {
            var result = YamlRead();
            UnityEngine.Debug.Log($"Yaml_Read [{result.success}] - {result.elapsedTime}");
        }

        public async void YamlWriteAsync_Click()
        {
            var result = await YamlWriteAsync();
            UnityEngine.Debug.Log($"Yaml_WriteAsync [{result.success}] - {result.elapsedTime}");
        }

        public async void YamlReadAsync_Click()
        {
            var result = await YamlReadAsync();
            UnityEngine.Debug.Log($"Yaml_ReadAsync [{result.success}] - {result.elapsedTime}");
        }

        public void BinaryWrite_Click()
        {
            var result = BinaryWrite();
            UnityEngine.Debug.Log($"Binary_Write [{result.success}] - {result.elapsedTime}");
        }

        public void BinaryRead_Click()
        {
            var result = BinaryRead();
            UnityEngine.Debug.Log($"Binary_Read [{result.success}] - {result.elapsedTime}");
        }

        public async void BinaryWriteAsync_Click()
        {
            var result = await BinaryWriteAsync();
            UnityEngine.Debug.Log($"Binary_WriteAsync [{result.success}] - {result.elapsedTime}");
        }

        public async void BinaryReadAsync_Click()
        {
            var result = await BinaryReadAsync();
            UnityEngine.Debug.Log($"Binary_ReadAsync [{result.success}] - {result.elapsedTime}");
        }

        #endregion

        [Serializable]
        [MessagePackObject(true)]
        private class TestObject
        {
            public string PlayerName { get; set; }
            public bool ModsEnabled { get; set; } = true;
            public int Kills { get; set; } = 353235;
            public double Exp { get; set; } = 1122334455667788.8877665544332211;
            public Dictionary<string, float> Location { get; set; } = new Dictionary<string, float>();
            public List<string> ItemIDs { get; set; } = new List<string>();
            public int[] HighestSeasonalLevels { get; set; } = {7, 4, 6, 2, 9};

            public TestObject()
            {
                Location["X"] = 250.25f;
                Location["Y"] = 72f;
                Location["Z"] = 3259.65f;

                ItemIDs.Add("silverSword");
            }

            public override bool Equals(object obj)
            {
                try
                {
                    var newObj = (TestObject) obj;
                    if (newObj != null && newObj.PlayerName == PlayerName && newObj.ModsEnabled == ModsEnabled &&
                        newObj.Kills == Kills &&
                        !newObj.ItemIDs.Except(ItemIDs).Any() && !ItemIDs.Except(newObj.ItemIDs).Any())
                        return true;
                }
                catch (Exception e)
                {
                    return false;
                }

                return false;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Player Name: {PlayerName}");
                sb.AppendLine($"Mods Enabled: {ModsEnabled.ToString()}");
                sb.AppendLine($"Kills: {Kills.ToString()}");
                sb.AppendLine($"Exp: {Exp.ToString()}");

                sb.AppendLine($"Item IDs: {ItemIDs.Count}");
                foreach (var VARIABLE in ItemIDs)
                {
                    sb.AppendLine($"\t{VARIABLE}");
                }

                sb.AppendLine($"Highest Seasonal Levels: {HighestSeasonalLevels.Length}");
                foreach (var VARIABLE in HighestSeasonalLevels)
                {
                    sb.AppendLine($"\t{VARIABLE}");
                }

                return sb.ToString();
            }
        }

        private class TestResult
        {
            public bool success;
            public string elapsedTime;

            public TestResult(bool success, TimeSpan ts)
            {
                this.success = success;
                this.elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
            }
        }
    }
}