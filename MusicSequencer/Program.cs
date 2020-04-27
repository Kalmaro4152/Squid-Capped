using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace MusicSequencer
{
    public enum Notes
    {
        NONE,
        WHOLE,
        DOTTEDHALF,
        HALF,
        DOTTEDQUATER,
        QUARTER,
        DOTTEDEIGTH,
        EIGTH,
        SIXTEENTH,
        END
    }
    
    public enum Pitches
    {
        A1, A2, A3, A4, A5, A6, A7, A8,
        AS1, AS2, AS3, AS4, AS5, AS6, AS7,
        B1, B2, B3, B4, B5, B6, B7,
        C2, C3, C4, C5, C6, C7, C8,
        CS2, CS3, CS4, CS5, CS6, CS7, CS8,
        D2, D3, D4, D5, D6, D7, D8,
        DS1, DS2, DS3, DS4, DS5 , DS6, DS7, DS8,
        E1, E2, E3, E4, E5, E6, E7, E8,
        F1, F2, F3, F4, F5, F6, F7, F8,
        FS1, FS2, FS3, FS4, FS5, FS6, FS7, FS8,
        G1, G2, G3, G4, G5, G6, G7, G8,
        GS1, GS2, GS3, GS4, GS5, GS6, GS7, GS8,
        REST, END,
        NONE
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            List<Notes> songNotes = new List<Notes>();
            List<Pitches> songPitches = new List<Pitches>();
            
            Dictionary<Pitches, int> definedPitches = new Dictionary<Pitches, int>();
            InitializePitched(definedPitches);
            
            DisplayMainMenu(songNotes, songPitches, definedPitches);
        }

        private static void DisplayMainMenu(List<Notes> songNotes, List<Pitches> songPitches, Dictionary<Pitches, int> definedPitches)
        {
            bool exitMenu = false;
            int BPM = 60;

            do
            {
                WriteLine("\t\t The Music Sequencer");
                WriteLine("\t A program designed to explore the basic components of how a midi sequencer works.");
                WriteLine();
                WriteLine("1) Load a saved file [NOT CURRENTLY SUPPORTED]");
                WriteLine("2) Create a new composition");
                WriteLine("3) Play a Composition");
                WriteLine("4) Save composition to file");
                WriteLine("q) Quit Program without saving");
                Write("Menu Choice >>");

                switch (ReadLine())
                {
                    case "1": WriteLine("Not Currently Supported"); DisplayMenuPrompt("the Main Menu"); break;
                    case "2": (songNotes, songPitches, BPM) = CreateNewPiece(songNotes, songPitches); break;
                    case "3": PlayPiece(songNotes, songPitches, BPM, definedPitches); break;
                    case "4": SavePiece(songNotes, songPitches, BPM); break;
                    case "q": WriteLine("Bye! I'll miss you!"); WriteLine("Press any key to say Goodbye!"); ReadKey(); exitMenu = true; break;
                    default: WriteLine("Please choose one of the listed options. \nPress any key to Continue."); ReadKey(); break;
                }

            } while (exitMenu == false);
        }

        private static (List<Notes>, List<Pitches>, int) CreateNewPiece(List<Notes> songNotes, List<Pitches> songPitches)
        {
            bool validRes = false;
            int noteCount = 1;
            int pitchCount = 1;
            int BPM;
            bool menuVar = false;
            Notes notes = Notes.NONE;
            Pitches pitches = Pitches.NONE;
            
            DisplayScreenHeader("Create a New Piece");
            WriteLine("CreateNewPiece Method");
            WriteLine("Create a new musical piece in 4/4 time. Does not support editing or undo. Sorry.");
            
            WriteLine("What is the BPM?");
            WriteLine("Note: Will work best with BPMs divisible by 3.");
            Write("BPM >>");
            
            do
            {
                if (int.TryParse(ReadLine(), out BPM)) { WriteLine(); validRes = true; }
                else WriteLine("Please use only whole numbers.\n");
            } while (!validRes);

            WriteLine("Please chose the duration of a note, and the pitch of it. Note: These will be played in the order dictated.");
            WriteLine("Notes Available:");
            foreach (string note in Enum.GetNames(typeof(Notes)))
            {
                Write($"-{note.ToLower()}\t");
                if (noteCount % 5 == 0) Write("\n");
                noteCount++;
            }
            WriteLine();
            WriteLine("Pitches Available:");
            foreach (string pitch in Enum.GetNames(typeof(Pitches)))
            {
                Write($"-{pitch}\t");
                if (pitchCount % 7 == 0) Write("\n");
                noteCount++;
            }
            WriteLine();

            do
            {
                WriteLine("[End] will end the creator and the song.");
                Write("Please enter a Duration: ");
                validRes = Enum.TryParse(ReadLine().ToUpper(), out notes);
                if (notes == Notes.END) menuVar = true;

                if (validRes && menuVar == false)
                {
                    songNotes.Add(notes);
                    WriteLine();
                    do
                    {
                        WriteLine("[Rest] will make this note a rest.");
                        Write("Please enter a Note Pitch: ");

                        validRes = Enum.TryParse(ReadLine().ToUpper(), out pitches);
                        if (validRes == true)
                        {
                            songPitches.Add(pitches);
                            WriteLine();
                        }
                        else
                        {
                            WriteLine("Please enter a pitch from the list above.");
                            WriteLine();
                        }
                    } while (validRes != true);
                }
                else if (validRes && notes == Notes.END) { songNotes.Add(notes); songPitches.Add(Pitches.END); menuVar = true; WriteLine("\nCreator Ended."); }
                else
                {
                    WriteLine("Please enter a Duration from the list above.");
                    WriteLine();
                }
            } while (menuVar == false);
            
            DisplayMenuPrompt("the Main Menu");
            return (songNotes, songPitches, BPM);
        }

        private static void PlayPiece(List<Notes> songNotes, List<Pitches> songPitches, int BPM, Dictionary<Pitches, int> definedPitches)
        {
            int currentPitch = 0;
            
            DisplayScreenHeader("Play Composed Piece");
            WriteLine("PlayPiece Method");

            WriteLine("Play a Loaded or Composed Piece");
            WriteLine("Press any key to start");
            ReadKey();
            
            foreach (Notes note in songNotes)
            {
                definedPitches.TryGetValue(songPitches[currentPitch], out int frequency);
                switch (note)
                {
                    case Notes.END: WriteLine("Take a Bow, the piece is finished."); break;
                    case Notes.WHOLE: WholeNote(BPM, frequency); break;
                    case Notes.DOTTEDHALF: DottedHalf(BPM, frequency); break;
                    case Notes.HALF: Half(BPM, frequency); break;
                    case Notes.DOTTEDQUATER: DottedQuarter(BPM, frequency); break;
                    case Notes.QUARTER: Quarter(BPM, frequency); break;
                    case Notes.DOTTEDEIGTH: DottedEigth(BPM, frequency); break;
                    case Notes.EIGTH: Eigth(BPM, frequency); break;
                    case Notes.SIXTEENTH: Sixteenth(BPM, frequency); break;
                    case Notes.NONE: WriteLine("Ya done broke it."); break;
                }
                currentPitch += 1;
            }
            DisplayMenuPrompt("the Main Menu");
        }

        private static void SavePiece(List<Notes> songNotes, List<Pitches> songPitches, int BPM)
        {
            int i = 0;
            DisplayScreenHeader("Save your Composition");
            WriteLine("SavePiece Method");
            
            WriteLine("Saves your composition to the Debug/Data file.");
            WriteLine("Press any key to save.");
            ReadKey();
            
            string dataPath = @"MyNotes.txt";
            foreach (Notes note in songNotes)
            {
                File.AppendAllText("MyNotes.txt", songNotes[i].ToString() + "\n");
                i += 1;
            }
            i = 0;
            dataPath = @"MyPitches.txt";
            foreach (Pitches pitch in songPitches)
            {
                File.AppendAllText("MyPitches.txt", songPitches[i].ToString() + "\n");
            }
            i = 0;
            dataPath = @"MyBPM.txt";
            File.AppendAllText(dataPath, BPM + "\n");
            
            WriteLine("Information written to MyNotes.txt, MyPitches.txt, and MyBPM.txt");
            WriteLine("Change the name of the file to prevent it from being overwritten.");

            DisplayMenuPrompt("the Main Menu");
        }

        private static void DisplayMenuPrompt(string menuName)
        {
            WriteLine($"Press Any Key to return to {menuName}.");
            ReadKey();
            Clear();
        }

        private static void DisplayScreenHeader(string header)
        {
            Clear();
            WriteLine($"\t\t{header}");
        }
        
        private static void InitializePitched(Dictionary<Pitches, int> definedPitches)
        {
            definedPitches.Add(Pitches.A1, 55); definedPitches.Add(Pitches.A2, 110); definedPitches.Add(Pitches.A3, 220); definedPitches.Add(Pitches.A4, 440); definedPitches.Add(Pitches.A5, 880); definedPitches.Add(Pitches.A6, 1760); definedPitches.Add(Pitches.A7, 3520); definedPitches.Add(Pitches.A8, 7040);
            definedPitches.Add(Pitches.AS1, 58); definedPitches.Add(Pitches.AS2, 116); definedPitches.Add(Pitches.AS3, 233); definedPitches.Add(Pitches.AS4, 466); definedPitches.Add(Pitches.AS5, 932); definedPitches.Add(Pitches.AS6, 1865); definedPitches.Add(Pitches.AS7, 3729);
            definedPitches.Add(Pitches.B1, 62); definedPitches.Add(Pitches.B2, 123); definedPitches.Add(Pitches.B3, 247); definedPitches.Add(Pitches.B4, 494); definedPitches.Add(Pitches.B5, 989); definedPitches.Add(Pitches.B6, 1976); definedPitches.Add(Pitches.B7, 3951);
            definedPitches.Add(Pitches.C2, 65); definedPitches.Add(Pitches.C3, 131); definedPitches.Add(Pitches.C4, 262); definedPitches.Add(Pitches.C5, 523); definedPitches.Add(Pitches.C6, 1046); definedPitches.Add(Pitches.C7, 2093); definedPitches.Add(Pitches.C8, 4186);
            definedPitches.Add(Pitches.CS2, 69); definedPitches.Add(Pitches.CS3, 138); definedPitches.Add(Pitches.CS4, 277); definedPitches.Add(Pitches.CS5, 554); definedPitches.Add(Pitches.CS6, 1109); definedPitches.Add(Pitches.CS7, 2217); definedPitches.Add(Pitches.CS8, 4435);
            definedPitches.Add(Pitches.D2, 73); definedPitches.Add(Pitches.D3, 147); definedPitches.Add(Pitches.D4, 294); definedPitches.Add(Pitches.D5, 587); definedPitches.Add(Pitches.D6, 1175); definedPitches.Add(Pitches.D7, 2349); definedPitches.Add(Pitches.D8, 4699);
            definedPitches.Add(Pitches.DS1, 39); definedPitches.Add(Pitches.DS2, 78); definedPitches.Add(Pitches.DS3, 155); definedPitches.Add(Pitches.DS4, 311); definedPitches.Add(Pitches.DS5, 622); definedPitches.Add(Pitches.DS6, 1244); definedPitches.Add(Pitches.DS7, 2489); definedPitches.Add(Pitches.DS8, 4978);
            definedPitches.Add(Pitches.E1, 41); definedPitches.Add(Pitches.E2, 82); definedPitches.Add(Pitches.E3, 165); definedPitches.Add(Pitches.E4, 330); definedPitches.Add(Pitches.E5, 659); definedPitches.Add(Pitches.E6, 1318); definedPitches.Add(Pitches.E7, 2637); definedPitches.Add(Pitches.E8, 5274);
            definedPitches.Add(Pitches.F1, 44); definedPitches.Add(Pitches.F2, 87); definedPitches.Add(Pitches.F3, 175); definedPitches.Add(Pitches.F4, 349); definedPitches.Add(Pitches.F5, 698); definedPitches.Add(Pitches.F6, 1397); definedPitches.Add(Pitches.F7, 2794); definedPitches.Add(Pitches.F8, 5588);
            definedPitches.Add(Pitches.FS1, 46); definedPitches.Add(Pitches.FS2, 92); definedPitches.Add(Pitches.FS3, 185); definedPitches.Add(Pitches.FS4, 367); definedPitches.Add(Pitches.FS5, 740); definedPitches.Add(Pitches.FS6, 1480); definedPitches.Add(Pitches.FS7, 2960); definedPitches.Add(Pitches.FS8, 5920);
            definedPitches.Add(Pitches.G1, 49); definedPitches.Add(Pitches.G2, 98); definedPitches.Add(Pitches.G3, 196); definedPitches.Add(Pitches.G4, 392); definedPitches.Add(Pitches.G5, 784); definedPitches.Add(Pitches.G6, 1568); definedPitches.Add(Pitches.G7, 3136); definedPitches.Add(Pitches.G8, 6272);
            definedPitches.Add(Pitches.GS1, 52); definedPitches.Add(Pitches.GS2, 104); definedPitches.Add(Pitches.GS3, 208); definedPitches.Add(Pitches.GS4, 415); definedPitches.Add(Pitches.GS5, 831); definedPitches.Add(Pitches.GS6, 1661); definedPitches.Add(Pitches.GS7, 3322); definedPitches.Add(Pitches.GS8, 6645);
            definedPitches.Add(Pitches.REST, 32700); definedPitches.Add(Pitches.NONE, 0); definedPitches.Add(Pitches.END, 32700);
        }

        private static void WholeNote(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000));
            Beep(frequency, dur);
            WriteLine("Note Played");
        }
        
        private static void DottedHalf(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000)); 
            Beep(frequency, dur);
            WriteLine("Note Played");
        }
        
        private static void Half(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000)); 
            Beep(frequency, dur);
            WriteLine("Note Played");
        }
        
        private static void DottedQuarter(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000)); 
            Beep(frequency, dur);
            WriteLine("Note Played");            
        }
        
        private static void Quarter(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000)); 
            Beep(frequency, dur);
            WriteLine("Note Played");
        }
        
        private static void DottedEigth(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000)); 
            Beep(frequency, dur);
            WriteLine("Note Played");
        }
        
        private static void Eigth(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000)); 
            Beep(frequency, dur);
            WriteLine("Note Played");
        }

        private static void Sixteenth(int BPM, int frequency)
        {
            double dur1 = 5 * (BPM / 3);
            int dur = Convert.ToInt32(Math.Round(dur1 * 1000));
            Beep(frequency, dur);
            WriteLine("Note Played");
        }
    }
}
