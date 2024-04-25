using ATMApp.Domain.Entities;
using ATMapp.UI;
using System;
using System.Collections.Generic;
using ATMapp;

namespace ATMapp.App
{
    class Entry
    {
        static void Main(string[] args)
        {

            ATMapp atmApp = new ATMapp();
            atmApp.InitializeData();
            atmApp.Run();
        }
    }
}