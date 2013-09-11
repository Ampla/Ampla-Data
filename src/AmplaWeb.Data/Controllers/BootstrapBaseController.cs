﻿using System.Web.Mvc;
using AmplaWeb.Data.BootstrapSupport;

namespace AmplaWeb.Data.Controllers
{
    public class BootstrapBaseController : Controller
    {
        public void Attention(string message)
        {
            TempData.Add(Alerts.Attention, message);
        }

        public void Success(string message)
        {
            TempData.Add(Alerts.Success, message);
        }

        public void Information(string message)
        {
            TempData.Add(Alerts.Information, message);
        }

        public void Error(string message)
        {
            TempData.Add(Alerts.Error, message);
        }
    }
}
