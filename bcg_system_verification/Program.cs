﻿using System;
using System.Text;
using bcg_system_verification.verifiers;
using bcg_system_verification.http;

namespace bcg_system_verification
{
    class Program
    {
        static void Main(string[] args)
        {

            UnhandledExceptionHandler handler = new UnhandledExceptionHandler();

            AppDomain.CurrentDomain.UnhandledException += 
                new UnhandledExceptionEventHandler(
                    handler.Application_Exception);

            //remove me when done
            Globals.debugMode = true;
            Debug.writeHeader("Mode Debugging");

            //Startup
            Globals.init();
            Arguments.check(args);

            //Primary App
            App();
        }

        private static void App(){
            //Connect to web server to grab uuid and ip
            get.start();

            firewall.Verify();
            antivirus.Verify();
            network.Verify();

            post.final();
            //Debugging console
            if (Globals.debugMode) Debug.results();         
        }
        
    }

    /// 
    /// Handles an unhandled exception.
    /// 
    internal class UnhandledExceptionHandler
    {
        /// 
        /// Handles the exception.
        /// 
        public void Application_Exception(
            object sender, UnhandledExceptionEventArgs args)
        {

            Exception e = (Exception)args.ExceptionObject;

            string errorMessage = "UnhandledException:\n\n" +
                e.Message + "\n\n" +
                e.GetType() + "\n\nStack Trace:\n" +
                e.StackTrace;

            Console.WriteLine(errorMessage);

            // Prevents annoying Windows 7 crash message
            Environment.Exit(1);
        }

    } // End UnhandledExceptionHandler
}
