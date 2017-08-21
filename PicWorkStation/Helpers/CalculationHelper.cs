﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PicWorkStation
{

    public class CalculationInfo
    {
        public string WidthHeight { get; set; } 
        public string Thinkness { get; set; }
        public bool IsFillup { get; set; }
        public bool IsDoubleBottle { get; set; }
        public string AreaperBottle { get; set; }
    }

    public static class CalculationHelper
    {
        private static readonly string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"CalculationInfo\CalProtocols.xml";

        /// <summary>
        /// 加载协议
        /// </summary>
        public static IList<CalculationInfo> LoadAllCalculationInfos()
        {
            IList<CalculationInfo> allCalculationInfos = new List<CalculationInfo>();
            if (!File.Exists(FilePath))
            {
                throw new Exception(string.Format("File {0} not exist!", FilePath));
            }

            var xmlElement = XElement.Load(FilePath);
            foreach (var xElement in xmlElement.Elements("Item"))
            {
                var calculationInfo = new CalculationInfo();
                calculationInfo.WidthHeight = xElement.Attribute("WidthHeight").Value;
                calculationInfo.Thinkness = xElement.Attribute("Thinkness").Value;
                calculationInfo.IsFillup = xElement.Attribute("IsFillup").Value == "True";
                calculationInfo.IsDoubleBottle = xElement.Attribute("IsDoubleBottle").Value == "True";
                calculationInfo.AreaperBottle = xElement.Attribute("AreaperBottle").Value;
                allCalculationInfos.Add(calculationInfo);
            }
            return allCalculationInfos;
        }

        /// <summary>
        /// 保存协议
        /// </summary>
        public static void SaveAllCalculationInfos(IList<CalculationInfo> allCalculationInfos)
        {
            var xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                         new XElement("ItemList", new XAttribute("Version", "1.0")));

            foreach (var calculationInfo in allCalculationInfos)
            {
                var calculationInfoElement = new XElement("Item", new XAttribute("WidthHeight", calculationInfo.WidthHeight),
                       new XAttribute("Thinkness", calculationInfo.Thinkness),
                       new XAttribute("IsFillup", calculationInfo.IsFillup.ToString()),
                       new XAttribute("IsDoubleBottle", calculationInfo.IsDoubleBottle.ToString()),
                       new XAttribute("AreaperBottle", calculationInfo.AreaperBottle));

            }
            xmlDoc.Save(FilePath);
        }

        /// <summary>
        /// 瓷砖的长宽
        /// </summary>
        public static IList<string> GetAllWidthHeight(IList<CalculationInfo> allCalculationInfos)
        {
            var allWidthHeight = new List<string>();
            foreach (var calculationInfo in allCalculationInfos)
            {
                if (!allWidthHeight.Contains(calculationInfo.WidthHeight))
                {
                    allWidthHeight.Add(calculationInfo.WidthHeight);
                }
            }
            return allWidthHeight;
        }

        /// <summary>
        /// 缝隙大小
        /// </summary>
        public static IList<string> GetAllThinkness(IList<CalculationInfo> allCalculationInfos)
        {
            var allThinkness = new List<string>();
            foreach (var calculationInfo in allCalculationInfos)
            {
                if (!allThinkness.Contains(calculationInfo.Thinkness))
                {
                    allThinkness.Add(calculationInfo.Thinkness);
                }
            }
            return allThinkness;
        }

    }
}
