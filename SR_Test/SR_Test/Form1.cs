﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;


namespace SR_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            initRS();
            initTTS();
        }

        public void initRS()
        {
            try
            {
                SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new CultureInfo("ko-KR"));

                Grammar g = new Grammar("input.xml");
                sre.LoadGrammar(g);
                
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception e)
            {
                label1.Text = "init RS Error : " + e.ToString();
            }
        }

        SpeechSynthesizer tts;

        public void initTTS()
        {
            try 
            {
                tts = new SpeechSynthesizer();
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)");
                tts.SetOutputToDefaultAudioDevice();
                tts.Volume = 100;
            }
            catch(Exception e)
            {
                label1.Text = "init TTS Error : " + e.ToString();
            }
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            label1.Text = e.Result.Text;

            switch(e.Result.Text)
            {
                case "컴퓨터":
                    tts.SpeakAsync("네 컴퓨터입니다");
                    break;

                case "안녕":
                    tts.SpeakAsync("반갑습니다 음성인식 테스터입니다");
                    break;

                case "종료":
                    {
                        tts.Speak("프로그램을 종료합니다");
                        Application.Exit();
                        break;
                    }

                case "계산기":
                    {
                        tts.SpeakAsync("계산기를 실행합니다");
                        doProgram("c:\\windows\\system32\\calc.exe", "");
                        break;
                    }
               
                case "메모장":
                    {
                        tts.SpeakAsync("메모장을 실행합니다");
                        doProgram("c:\\windows\\system32\\notepad.exe", "");
                        break;
                    }

                case "콘솔":
                    {
                        tts.SpeakAsync("콘솔을 실행합니다");
                        doProgram("c:\\windows\\system32\\cmd.exe", "");
                        break;
                    }

                case "그림판":
                    {
                        tts.SpeakAsync("그림판을 실행합니다");
                        doProgram("c:\\windows\\system32\\mspaint.exe", "");
                        break;
                    }

                case "계산기 닫기":
                    {
                        tts.SpeakAsync("계산기를 종료합니다");
                        closeProcess("calc");
                        break;
                    }
            }
        }

        // 프로세스 실행
        private static void doProgram(string filename, string arg)
        {
            ProcessStartInfo psi;
            if(arg.Length != 0)
                psi = new ProcessStartInfo(filename, arg);
            else
                psi = new ProcessStartInfo(filename);
            Process.Start(psi);
        }

        // 프로세스 종료
        private static void closeProcess(string filename)
        {
            Process[] myProcesses;
            // Returns array containing all instances of Notepad.
            myProcesses = Process.GetProcessesByName(filename);
            foreach (Process myProcess in myProcesses)
            {
                myProcess.CloseMainWindow();
            }
        }
    }
}
