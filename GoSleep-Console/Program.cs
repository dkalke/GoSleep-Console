// Copyright 2024 Ko, Hung-Yu
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Win32;
using System;
using System.Diagnostics;


namespace GoSleep_Console
{
    internal class Program
    {
        static private void ApplySetting()
        {
            try
            {
                // 要執行的程序名稱
                string processName = "atbroker.exe";

                // 使用 Process.Start 啟動進程
                Process.Start(processName);
                Console.WriteLine($"{processName} started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (args[0] == "on")
                {
                    using (RegistryKey accessibilityKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Accessibility", true))
                        accessibilityKey.SetValue("Configuration", "colorfiltering");

                    using (RegistryKey colorFilteringRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\ColorFiltering"))
                        colorFilteringRegistryKey.SetValue("Active", 1, RegistryValueKind.DWord);

                    ApplySetting();
                }
                else if (args[0] == "off")
                {
                    using (RegistryKey colorFilteringRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\ColorFiltering"))
                        colorFilteringRegistryKey.SetValue("Active", 0, RegistryValueKind.DWord);

                    using (RegistryKey accessibilityKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Accessibility", true))
                        accessibilityKey.SetValue("Configuration", "");

                    using (RegistryKey accessibilityKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Accessibility\ATConfig\colorfiltering"))
                    {
                        if (accessibilityKey!=null)
                            Registry.CurrentUser.DeleteSubKeyTree(@"Software\Microsoft\Windows NT\CurrentVersion\Accessibility\ATConfig\colorfiltering");
                    }

                    ApplySetting();
                }
            }
        }
    }
}
