using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using LutLib.Controllers;
using LutTool.View;

namespace LutLib.View
{
    public class MainView : BaseView {
        private string _inputText;
        private ControllerType _selectedController;

        public MainView()
        {
            ParseCommand = new Command(() =>
            {
                
                if (ExpectedLength != CurrentLength) 
                    return;
                var bytes = InputBytes;
                Groups.Clear();
                var groups = SelectedController.ParseValues(bytes.ToArray());

                if (!bytes.SequenceEqual(SelectedController.CompileToLut(groups.ToArray())))
                {
                    var nn = SelectedController.CompileToLut(groups.ToArray());
                    for(var p = 0; p < bytes.Length; p++)
                        if (bytes[p] != nn[p])
                        {

                        }
                }
              
                
                var index = 0;
                foreach(var group in groups)
                    Groups.Add(new LutGroupView(index++,group, SelectedController));

                OnPropertyChanged(nameof(HasGroups));
            });

            CompileCommand = new Command(() =>
            {
                CompiledText = null;
                var saved = SelectedController.CompileToLut(Groups.Select(pX=>pX.Group).ToArray());
                var sb = new StringBuilder();
                var index = 0;
                foreach (var b in saved)
                {
                    if (index > 0 && index % 10 == 0) 
                        sb.AppendLine();
                   
                    sb.Append($"0x{b.ToString("X2")}, ");
                    index++;
                }

                InputText = sb.ToString();
                CompiledText = sb.ToString();
            });
            SelectedController = AvailableControllers.First();
        }


        public byte[] InputBytes
        {
            get
            {
                if (InputText == null) return Array.Empty<byte>();
                var byteStrs = InputText.Split(' ', ',').Select(pX => pX.Trim())
                    .Where(pX => !string.IsNullOrWhiteSpace(pX));
                var bytes = new List<byte>();
                try
                {

                    foreach (var str in byteStrs)
                    {
                        if (str.StartsWith("0x"))
                        {
                            bytes.Add(byte.Parse(str.Substring(2), NumberStyles.HexNumber));
                        }
                        else
                            bytes.Add(byte.Parse(str));

                    }
                }
                catch (Exception)
                {

                } 

                return bytes.ToArray();
            }
        }

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
                OnPropertyChanged(nameof(CurrentLength));
            }
        }

        public string CompiledText
        {
            get;set;
        }


        public ControllerType SelectedController
        {
            get => _selectedController;
            set
            {
                _selectedController = value;
                OnPropertyChanged(nameof(SelectedController));
                OnPropertyChanged(nameof(ExpectedLength));
                OnPropertyChanged(nameof(CurrentLength));
                Groups.Clear();
            }
        }

       

        public int ExpectedLength => SelectedController.LutLength;
        public int CurrentLength => InputBytes.Length;
        public ControllerType[] AvailableControllers { get; } = new ControllerType[]
        {
            new Ssd1680Controller(),
            new Ssd1681Controller(),
            new Ssd1675AController(),
            new Ssd1675BController(),
            new Ssd1677Controller(),
            new Ssd1619AController(),
        };


        public Command CompileCommand { get; set; }
        public Command ParseCommand { get; set; }

        public ObservableCollection<LutGroupView> Groups { get; } = new();

        public bool HasGroups => Groups.Any();
    }
}