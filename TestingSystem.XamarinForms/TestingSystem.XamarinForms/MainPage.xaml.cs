﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystem.XamarinForms.ApiServices;
using Xamarin.Forms;

namespace TestingSystem.XamarinForms
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private SubjectService service = new SubjectService();
        public MainPage()
        {
            InitializeComponent();
            //BindingContext = service.GetAll().First();
        }
    }
}
