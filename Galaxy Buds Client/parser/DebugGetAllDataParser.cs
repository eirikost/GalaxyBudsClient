﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using Galaxy_Buds_Client.message;
using Galaxy_Buds_Client.model;
using Galaxy_Buds_Client.model.Constants;

namespace Galaxy_Buds_Client.parser
{
    class DebugGetAllDataParser : BaseMessageParser
    {
        public override SPPMessage.MessageIds HandledType => SPPMessage.MessageIds.MSG_ID_DEBUG_GET_ALL_DATA;
        
        readonly String[] _swMonth = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
        readonly String[] _swRelVer = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        readonly String[] _swVer = { "E", "U" };
        readonly String[] _swYear = { "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public String HardwareVersion { set; get; }
        public String SoftwareVersion { set; get; }
        public String LeftBluetoothAddress { set; get; }
        public String RightBluetoothAddress { set; get; }
        public String TouchSoftwareVersion { set; get; }
        public short LeftAcceleratorX { set; get; }
        public short LeftAcceleratorY { set; get; }
        public short LeftAcceleratorZ { set; get; }
        public short RightAcceleratorX { set; get; }
        public short RightAcceleratorY { set; get; }
        public short RightAcceleratorZ { set; get; }
        public short LeftProximity { set; get; }
        public short RightProximity { set; get; }
        [Postfix(Text = "°C")]
        public double LeftThermistor { set; get; }
        [Postfix(Text = "°C")]
        public double RightThermistor { set; get; }

        [Postfix(Text = "%")]
        public double LeftAdcSOC { set; get; }
        [Postfix(Text = "V")]
        public double LeftAdcVCell { set; get; }
        [Postfix(Text = "mA")]
        public double LeftAdcCurrent { set; get; }

        [Postfix(Text = "%")]
        public double RightAdcSOC { set; get; }
        [Postfix(Text = "V")]
        public double RightAdcVCell { set; get; }
        [Postfix(Text = "mA")]
        public double RightAdcCurrent { set; get; }
        
        public String LeftHall { set; get; }
        public String RightHall { set; get; }

        [Device(Model.BudsPlus)]
        public short LeftProximityOffset { set; get; }
        [Device(Model.BudsPlus)]
        public short RightProximityOffset { set; get; }
        [Device(Model.BudsPlus)]
        public byte MsgVersion { set; get; }
        [Device(Model.BudsPlus)]
        public short LeftTspAbs { set; get; }
        [Device(Model.BudsPlus)]
        public short RightTspAbs { set; get; }
        [Device(Model.BudsPlus)]
        public short LeftTspDiff0 { set; get; }
        [Device(Model.BudsPlus)]
        public short LeftTspDiff1 { set; get; }
        [Device(Model.BudsPlus)]
        public short LeftTspDiff2 { set; get; }
        [Device(Model.BudsPlus)]
        public short RightTspDiff0 { set; get; }
        [Device(Model.BudsPlus)]
        public short RightTspDiff1 { set; get; }
        [Device(Model.BudsPlus)]
        public short RightTspDiff2 { set; get; }
        [Device(Model.BudsPlus)]
        public short LeftPR { set; get; }
        [Device(Model.BudsPlus)]
        public short RightPR { set; get; }
        [Device(Model.BudsPlus)]
        public short LeftWD { set; get; }
        [Device(Model.BudsPlus)]
        public short RightWD { set; get; }
        [Device(Model.BudsPlus)]
        public byte LeftCradleFlag { set; get; }
        [Device(Model.BudsPlus)]
        public byte RightCradleFlag { set; get; }
        [Device(Model.BudsPlus)]
        public byte LeftCradleBatt { set; get; }
        [Device(Model.BudsPlus)]
        public byte RightCradleBatt { set; get; }

        public override void ParseMessage(SPPMessage msg)
        {
            if (msg.Id != HandledType)
                return;

            if (ActiveModel == Model.Buds)
            {

                int hw1 = (msg.Payload[0] & 240) >> 4;
                int hw2 = (msg.Payload[0] & 15);

                HardwareVersion = "rev" + hw1.ToString("X") + "." + hw2.ToString("X");
                SoftwareVersion = VersionDataToString(msg.Payload, 1, "R");
                TouchSoftwareVersion = msg.Payload[4].ToString("X");
                LeftBluetoothAddress = BytesToMacString(msg.Payload, 5);
                RightBluetoothAddress = BytesToMacString(msg.Payload, 11);

                LeftAcceleratorX = BitConverter.ToInt16(msg.Payload, 17);
                LeftAcceleratorY = BitConverter.ToInt16(msg.Payload, 19);
                LeftAcceleratorZ = BitConverter.ToInt16(msg.Payload, 21);
                RightAcceleratorX = BitConverter.ToInt16(msg.Payload, 23);
                RightAcceleratorY = BitConverter.ToInt16(msg.Payload, 25);
                RightAcceleratorZ = BitConverter.ToInt16(msg.Payload, 27);

                LeftProximity = BitConverter.ToInt16(msg.Payload, 29);
                RightProximity = BitConverter.ToInt16(msg.Payload, 31);

                LeftThermistor = BitConverter.ToDouble(msg.Payload, 33);
                RightThermistor = BitConverter.ToDouble(msg.Payload, 41);

                LeftAdcSOC = BitConverter.ToDouble(msg.Payload, 49);
                LeftAdcVCell = BitConverter.ToDouble(msg.Payload, 57);
                LeftAdcCurrent = BitConverter.ToDouble(msg.Payload, 65);
                RightAdcSOC = BitConverter.ToDouble(msg.Payload, 73);
                RightAdcVCell = BitConverter.ToDouble(msg.Payload, 81);
                RightAdcCurrent = BitConverter.ToDouble(msg.Payload, 89);

                LeftHall = msg.Payload[97].ToString("x") + " " + msg.Payload[98].ToString("x");
                RightHall = msg.Payload[99].ToString("x") + " " + msg.Payload[100].ToString("x");
            }
            else
            {
                int hw1 = (msg.Payload[1] & 240) >> 4;
                int hw2 = (msg.Payload[1] & 15);

                MsgVersion = msg.Payload[0];

                HardwareVersion = "rev" + hw1.ToString("X") + "." + hw2.ToString("X");
                SoftwareVersion = VersionDataToString(msg.Payload, 2, "R");
                TouchSoftwareVersion = msg.Payload[5].ToString("X");
                LeftBluetoothAddress = BytesToMacString(msg.Payload, 6);
                RightBluetoothAddress = BytesToMacString(msg.Payload, 12);

                LeftAcceleratorX = BitConverter.ToInt16(msg.Payload, 18);
                LeftAcceleratorY = BitConverter.ToInt16(msg.Payload, 20);
                LeftAcceleratorZ = BitConverter.ToInt16(msg.Payload, 22);
                RightAcceleratorX = BitConverter.ToInt16(msg.Payload, 24);
                RightAcceleratorY = BitConverter.ToInt16(msg.Payload, 26);
                RightAcceleratorZ = BitConverter.ToInt16(msg.Payload, 28);

                LeftProximity = BitConverter.ToInt16(msg.Payload, 30);
                LeftProximityOffset = BitConverter.ToInt16(msg.Payload, 32);
                RightProximity = BitConverter.ToInt16(msg.Payload, 34);
                RightProximityOffset = BitConverter.ToInt16(msg.Payload, 36);

                LeftThermistor = BitConverter.ToInt16(msg.Payload, 38) * 0.1d;
                RightThermistor = BitConverter.ToInt16(msg.Payload, 40) * 0.1d;

                LeftAdcSOC = BitConverter.ToInt16(msg.Payload, 42);
                LeftAdcVCell = BitConverter.ToInt16(msg.Payload, 44) * 0.01d;
                LeftAdcCurrent = BitConverter.ToInt16(msg.Payload, 46) * 1.0E-4d;
                RightAdcSOC = BitConverter.ToInt16(msg.Payload, 48);
                RightAdcVCell = BitConverter.ToInt16(msg.Payload, 50) * 0.01d;
                RightAdcCurrent = BitConverter.ToInt16(msg.Payload, 52) * 1.0E-4d;

                LeftTspAbs = BitConverter.ToInt16(msg.Payload, 54);
                RightTspAbs = BitConverter.ToInt16(msg.Payload, 56);

                LeftTspDiff0 = BitConverter.ToInt16(msg.Payload, 58);
                LeftTspDiff1 = BitConverter.ToInt16(msg.Payload, 60);
                LeftTspDiff2 = BitConverter.ToInt16(msg.Payload, 62);
                RightTspDiff0 = BitConverter.ToInt16(msg.Payload, 64);
                RightTspDiff1 = BitConverter.ToInt16(msg.Payload, 66);
                RightTspDiff2 = BitConverter.ToInt16(msg.Payload, 68);

                LeftHall = msg.Payload[70].ToString("x");
                RightHall = msg.Payload[71].ToString("x");

                LeftPR = BitConverter.ToInt16(msg.Payload, 72);
                RightPR = BitConverter.ToInt16(msg.Payload, 74);
                LeftWD = BitConverter.ToInt16(msg.Payload, 76);
                RightWD = BitConverter.ToInt16(msg.Payload, 78);

                LeftCradleFlag = msg.Payload[79];
                RightCradleFlag = msg.Payload[80];

                LeftCradleBatt = msg.Payload[81];
                RightCradleBatt = msg.Payload[82];
            }
        }
        private String BytesToMacString(byte[] payload, int startIndex)
        {
            StringBuilder sb = new StringBuilder();
            for (int i13 = 0; i13 < 6; i13++)
            {
                if (i13 != 0)
                {
                    sb.Append(":");
                }
                sb.Append(((payload[i13 + startIndex] & 240) >> 4).ToString("X"));
                sb.Append((payload[i13 + startIndex] & 15).ToString("X"));
            }
            return sb.ToString();
        }

        private String VersionDataToString(byte[] payload, int startIndex, String side)
        {
            if (ActiveModel == Model.Buds)
            {
                int swVarIndex = payload[startIndex];
                int swYearIndex = (payload[startIndex + 1] & 240) >> 4;
                int swMonthIndex = payload[startIndex + 1] & 15;
                byte swRelVerIndex = payload[startIndex + 2];

                String swRelVarString;
                if (swRelVerIndex <= 15)
                {
                    swRelVarString = (swRelVerIndex & 255).ToString("X");
                }
                else
                {
                    swRelVarString = _swRelVer[swRelVerIndex - 16];
                }

                return side + "170XX" + _swVer[swVarIndex] + "0A" + _swYear[swYearIndex] + _swMonth[swMonthIndex] +
                       swRelVarString;
            }
            else
            {
                String swVar = payload[startIndex] == 0 ? "E" : "U";
                int swYearIndex = (payload[startIndex + 1] & 240) >> 4;
                int swMonthIndex = payload[startIndex + 1] & 15;
                byte swRelVerIndex = payload[startIndex + 2];

                return side + "175XX" + swVar + "0A" + _swYear[swYearIndex] + _swMonth[swMonthIndex] +
                       _swRelVer[swRelVerIndex]; ;
            }
        }

        public override Dictionary<String, String> ToStringMap()
        {
            Dictionary<String, String> map = new Dictionary<string, string>();
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "HandledType" || property.Name == "ActiveModel")
                    continue;

                var customAttributes = (PostfixAttribute[])property.GetCustomAttributes(typeof(PostfixAttribute), true);
                var customAttributesB = (DeviceAttribute[])property.GetCustomAttributes(typeof(DeviceAttribute), true);

                String valuePostfix = "";
                if (customAttributes.Length > 0)
                {
                    valuePostfix = customAttributes[0].Text;
                }

                if (customAttributesB.Length <= 0)
                {
                    map.Add(property.Name, property.GetValue(this).ToString() + valuePostfix);
                }
                else if (customAttributesB[0].Model == ActiveModel)
                {
                    map.Add($"{property.Name} ({customAttributesB[0].Model.ToString()})", property.GetValue(this).ToString() + valuePostfix);
                }
            }

            return map;
        }
    }
}