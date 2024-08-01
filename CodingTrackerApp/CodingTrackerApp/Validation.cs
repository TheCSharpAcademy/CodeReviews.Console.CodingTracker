using DataAcess;
using Model;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTrackerApp
{
    public class Validation
    {
        public static void StartEndValidation(bool isStartValid, bool isEndValid, string strStartInput, string strEndInput)
        {
            if (isStartValid && isEndValid)
            {
                DateTime startInput = Logic.DateTimeConstruct(strStartInput);
                DateTime endInput = Logic.DateTimeConstruct(strEndInput);
                double duration = Logic.CalculateDuration(startInput, endInput);

                if (duration > 0)
                {
                    using (MyDbContext db = new MyDbContext())
                    {
                        CodingSession codingSession = new CodingSession
                        {
                            Start = startInput,
                            End = endInput,
                            Duration = duration
                        };
                        db.CodingSessions.Add(codingSession);
                        db.SaveChanges();
                        AnsiConsole.Write(new Markup("[red]Added[/]\n"));
                    }
                }
                else
                {
                    AnsiConsole.Write(new Markup("\n[red]End < Start. Try again![/]\n"));
                    Thread.Sleep(1000);
                    Logic.InsertSession();
                }
            }
            else
            {
                AnsiConsole.Write(new Markup("\n[red]Invalid Entry. Try again![/]\n"));
                Thread.Sleep(1000);
                Logic.InsertSession();
            }
        }
        public static void StartValidation(MyDbContext db, bool isStartValid, int updateKey, string start)
        {
            if (isStartValid)
            {
                CodingSession? codingSession = db.CodingSessions.Find(updateKey);
                if (codingSession != null)
                {
                    DateTime newStart = Logic.DateTimeConstruct(start);
                    codingSession.Start = newStart;

                    double newDuration = Logic.CalculateDuration(newStart, codingSession.End);
                    if (newDuration > 0)
                    {
                        codingSession.Duration = newDuration;
                        db.SaveChanges();

                        AnsiConsole.Write(new Markup("\n[red]Updated[/]\n"));
                        Thread.Sleep(1000);
                        Logic.GetAllSession();
                    }
                    else
                    {
                        AnsiConsole.Write(new Markup("\n[red]End < Start. Try again![/]\n"));
                        Thread.Sleep(1000);
                        Logic.UpdateSession();
                        return;
                    }
                }
            }
            else
            {
                AnsiConsole.Write(new Markup("\n[red]Invalid Entry. Try again![/]\n"));
                Thread.Sleep(1000);
                Logic.UpdateSession();
                return;
            }
        }
        public static void EndValidation(MyDbContext db, bool isEndValid, int updateKey, string end)
        {
            if (isEndValid)
            {
                CodingSession? codingSession = db.CodingSessions.Find(updateKey);
                if (codingSession != null)
                {
                    DateTime newEnd = Logic.DateTimeConstruct(end);
                    codingSession.End = newEnd;

                    double newDuration = Logic.CalculateDuration(codingSession.Start, newEnd);
                    if (newDuration > 0)
                    {
                        codingSession.Duration = newDuration;
                        db.SaveChanges();

                        AnsiConsole.Write(new Markup("\n[red]Updated[/]\n"));
                        Thread.Sleep(1000);
                        Logic.GetAllSession();
                    }
                    else
                    {
                        AnsiConsole.Write(new Markup("\n[red]End < Start. Try again![/]\n"));
                        Thread.Sleep(1000);
                        Logic.UpdateSession();
                        return;
                    }   
                }
            }
            else
            {
                AnsiConsole.Write(new Markup("\n[red]Invalid Entry. Try again![/]\n"));
                Thread.Sleep(1000);
                Logic.UpdateSession();
                return;
            }
        }
    }
}
