﻿using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Shop_server
{
    internal static class Extensions
    {
        public static string ComputeSHA256(string rawData)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
            return new(bytes.SelectMany(x => x.ToString("x2").ToCharArray()).ToArray());
        }
    }
}