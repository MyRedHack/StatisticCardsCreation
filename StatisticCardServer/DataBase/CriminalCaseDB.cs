using Microsoft.Data.SqlClient;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System;
using System.Collections.Generic;
using StatCardApp;
using System.Reflection;
using System.Linq;
using StatCardApp.ViewModel;
using StatCardApp.Client;
using Newtonsoft.Json;
using StatisticCardServer;

namespace StatisticCardServer.DataBase
{
    public class CriminalCaseDB
    {
        public ServerException Exception { get; set; }

        public delegate void FCardDataProccessing(StatisticCard card);

        #region Обработка Ф-1

        public void F1CardDataProccessing(StatisticCard card)
        {
            CardF1 F1 = (CardF1)card;

            bool sameProceedingDate = CheckUniqueField($"SELECT proceedingDate FROM Cases WHERE number = {F1.CaseNumber}", DBNull.Value, typeof(DBNull));

            if (sameProceedingDate)
            {
                if (F1.StartCaseDate.HasValue)
                {
                    string query = $"UPDATE Cases SET proceedingDate = '{F1.StartCaseDate}' WHERE number = {F1.CaseNumber}";

                    ExecuteQuery(query);
                }
            }

            if (F1.Accounting != 9)
            {
                if (CheckUniqueField("SELECT id FROM Crimes", $"{F1.CaseNumber}{F1.CrimeNumber}", typeof(long)))
                {
                    Exception.Message = $"В базе данных уже имеется преступление с номером {F1.CrimeNumber.Value}";
                    Exception.HasError = true;
                    return;
                }

                InsertFullCrime(F1);
            }
            else
            {
                ChangeFullCrime(F1);
            }
        }

        private void InsertFullCrime(CardF1 F1)
        {
            InsertCrime(F1);
            InsertOwns(F1);
            InsertWeapons(F1);
            InsertThings(F1);
            InsertVictimCharacter(F1);
            InsertExtraInfo(F1);
            InsertSceneInspectionInvolvedPerson(F1);
            InsertTempCondition(F1);
        }

        private void InsertCrime(CardF1 F1)
        {
            string queryOperator = "INSERT Crimes(id, caseNumberFK, listedCrimeObject, listedProsecutorOffice, number, stateRegNumber, registrationDate, violationInfo, crimePerson," +
              "position, rank, fio, detectionRequirement, crimeDate, crimeDetectedObject, crimePretext, caseStartedObject, preInvestigationForm," +
              "transMaterialsDate, startCaseDate, acceptCaseForProccess, caseFrom, eventDescription, connectedCase, article, sign, part, paragraph, qualCrimeAttribute," +
              "crimeMotive, bribeSum, articlesBeforeLegalization, relatedCrimesQualification, finishedCompositionSign, characteristicOfGroup," +
              "linksOfGroup, accomplicesOfGroup, crimeCharacter, truamaCondition, unlawfullBenefit, unlawfullBenefitSum, crimePlace," +
              "crimeState, militaryUntDislocation, publicPlace, crimeMeans, sceneInspectionPerson, nationalProgramm, investigatorPosition, investigatorRank, investigatorFIO) VALUES";
            Console.WriteLine($"-----------------{F1.RegistrationDate}----------------------------");
            string queryData = $"({F1.CaseNumber}{F1.CrimeNumber}, {F1.CaseNumber}, '{F1.ListedCrimeObject}', '{F1.ListedProsecutorOffice}', {F1.CrimeNumber}, {F1.StateRegNumber}, '{F1.RegistrationDate}', {F1.ViolationInfo}, " +
             $"{F1.CrimePerson}, '{F1.CrimePersDescription.Position}', '{F1.CrimePersDescription.Rank}', '{F1.CrimePersDescription.FIO}', {F1.DetectionRequirement}, " +
             $"'{F1.CrimeDate}', {F1.CrimeDetectedObject}, {F1.CrimePretext}, {F1.CaseStartedObject}, {F1.PreInvestigationForm}, '{F1.TransMaterialsDate}', '{F1.StartCaseDate}', " +
             $"'{F1.AcceptCaseForProccess}', {F1.CaseFrom}, '{F1.EventDescription}', {F1.ConnectedCase}, {F1.CrimeQualification.Article}, '{F1.CrimeQualification.Sign}', '{F1.CrimeQualification.Part}', " +
             $"'{F1.CrimeQualification.Paragraph}', '{F1.QualCrimeAttribute}', {F1.CrimeMotive}, {F1.BribeSum}, '{F1.ArticlesBeforeLegalization}', '{F1.RelatedCrimesQualification}', " +
             $"{F1.FinishedCompositionSign}, {F1.CrimeGroup.Characteristic}, {F1.CrimeGroup.Links}, {F1.CrimeGroup.Accomplices}, {F1.CrimeCharacter}, {F1.TruamaCondition}, {F1.UnlawfullBenefit.ValueCode}, " +
             $"{F1.UnlawfullBenefit.Sum}, {F1.CrimePlace}, '{F1.CrimeState}', {F1.MilitaryUntDislocation}, {F1.PublicPlace}, {F1.CrimeMeans}, {F1.SceneInspectionPerson}, '{F1.NationalProgramm}'," +
             $"'{KeyValueConvertor(F1.RegData.User.Position, "Positions")}', '{KeyValueConvertor(F1.RegData.User.Rank, "Ranks")}', '{F1.RegData.User.FIO}')";

            ExecuteQuery(queryOperator + queryData);
        }

        private void InsertOwns(CardF1 F1)
        {
            foreach (OE_Own own in F1.owns)
            {
                string query = $"INSERT INTO Owns(ownForm, ownType, count, crimeId) VALUES({own.OwnForm}, {own.OwnType}, {own.SumRub}, {F1.CaseNumber}{F1.CrimeNumber})";
                ExecuteQuery(query);
                Console.WriteLine(query);
            }
        }

        private void InsertWeapons(CardF1 F1)
        {
            foreach (OE_Weapon weapon in F1.weapons)
            {
                string query = $"INSERT INTO Weapons(code, count, crimeId) VALUES({weapon.Weapon}, {weapon.WCount}, {F1.CaseNumber}{F1.CrimeNumber})";
                ExecuteQuery(query);
                Console.WriteLine(query);
            }
        }

        private void InsertThings(CardF1 F1)
        {
            foreach (OE_ThingWithdrawed thing in F1.things)
            {
                string query = $"INSERT INTO ThingsWithdrawed(code, count, crimeId) VALUES({thing.Thing}, {thing.WCount}, {F1.CaseNumber}{F1.CrimeNumber})";
                ExecuteQuery(query);
                Console.WriteLine(query);
            }
        }

        private void InsertVictimCharacter(CardF1 F1)
        {
            foreach (El item in F1.VictimCharacter)
            {
                string query = $"INSERT INTO VictimCharacter(code, crimeId) VALUES({item.Num}, {F1.CaseNumber}{F1.CrimeNumber})";
                ExecuteQuery(query);
                Console.WriteLine(query);
            }
        }

        private void InsertExtraInfo(CardF1 F1)
        {
            foreach (El item in F1.ExtraInfo)
            {
                string query = $"INSERT INTO ExtraInfo(code, crimeId) VALUES({item.Num}, {F1.CaseNumber}{F1.CrimeNumber})";
                ExecuteQuery(query);
                Console.WriteLine(query);
            }
        }

        private void InsertTempCondition(CardF1 F1)
        {
            foreach (El item in F1.TempCondition)
            {
                string query = $"INSERT INTO TempCondition(code, crimeId) VALUES({item.Num}, {F1.CaseNumber}{F1.CrimeNumber})";
                ExecuteQuery(query);
                Console.WriteLine(query);
            }
        }

        private void InsertSceneInspectionInvolvedPerson(CardF1 F1)
        {
            foreach (El item in F1.SceneInspectionInvolvedPerson)
            {
                string query = $"INSERT INTO SceneInspectionInvolvedPerson(code, crimeId) VALUES({item.Num}, {F1.CaseNumber}{F1.CrimeNumber})";
                ExecuteQuery(query);
                Console.WriteLine(query);
            }
        }

        private void ChangeFullCrime(CardF1 F1)
        {
            ChangeCrime(F1);
            ChangeOwns(F1);
            ChangeWeapons(F1);
            ChangeThings(F1);
            ChangeVictimCharacter(F1);
            ChangeTempCondition(F1);
            ChangeExtraInfo(F1);
            ChangeSceneInspectionInvolvedPerson(F1);
        }

        private void ChangeCrime(CardF1 F1)
        {
            foreach (ChangedField item in F1.ChangedFields)
            {
                string query = $"UPDATE Crimes " +
                           $"SET {item.Field} = {item.Value} " +
                           $"WHERE id = {F1.CaseNumber}{F1.CrimeNumber}";

                Console.WriteLine(query);
                ExecuteQuery(query);
            }
        }

        private void ChangeOwns(CardF1 F1)
        {
            if (F1.owns.Count > 0)
            {
                DeletetOwns(F1);
                InsertOwns(F1);
            }
        }

        private void ChangeWeapons(CardF1 F1)
        {
            if (F1.weapons.Count > 0)
            {
                DeleteWeapons(F1);
                InsertWeapons(F1);
            }
        }

        private void ChangeThings(CardF1 F1)
        {
            if (F1.things.Count > 0)
            {
                DeleteThings(F1);
                InsertThings(F1);
            }
        }

        private void ChangeVictimCharacter(CardF1 F1)
        {
            if (F1.VictimCharacter.Count > 0)
            {
                DeleteVictimCharacter(F1);
                InsertVictimCharacter(F1);
            }
        }

        private void ChangeExtraInfo(CardF1 F1)
        {
            if (F1.ExtraInfo.Count > 0)
            {
                DeleteExtraInfo(F1);
                InsertExtraInfo(F1);
            }
        }

        private void ChangeTempCondition(CardF1 F1)
        {
            if (F1.TempCondition.Count > 0)
            {
                DeleteTempCondition(F1);
                InsertTempCondition(F1);
            }
        }

        private void ChangeSceneInspectionInvolvedPerson(CardF1 F1)
        {
            if (F1.SceneInspectionInvolvedPerson.Count > 0)
            {
                DeleteSceneInspectionInvolvedPerson(F1);
                InsertSceneInspectionInvolvedPerson(F1);
            }
        }

        private void DeletetOwns(CardF1 F1)
        {
            string query = $"DELETE FROM Owns WHERE crimeId = {F1.CaseNumber}{F1.CrimeNumber}";
            ExecuteQuery(query);
            Console.WriteLine(query);
        }

        private void DeleteWeapons(CardF1 F1)
        {
            string query = $"DELETE FROM Weapons WHERE crimeId = {F1.CaseNumber}{F1.CrimeNumber}";
            ExecuteQuery(query);
            Console.WriteLine(query);
        }

        private void DeleteThings(CardF1 F1)
        {
            string query = $"DELETE FROM ThingsWithdrawed WHERE crimeId = {F1.CaseNumber}{F1.CrimeNumber}";
            ExecuteQuery(query);
            Console.WriteLine(query);
        }

        private void DeleteVictimCharacter(CardF1 F1)
        {
            string query = $"DELETE FROM VictimCharacter WHERE crimeId = {F1.CaseNumber}{F1.CrimeNumber}";
            ExecuteQuery(query);
            Console.WriteLine(query);
        }

        private void DeleteExtraInfo(CardF1 F1)
        {
            string query = $"DELETE FROM ExtraInfo WHERE crimeId = {F1.CaseNumber}{F1.CrimeNumber}";
            ExecuteQuery(query);
            Console.WriteLine(query);
        }

        private void DeleteTempCondition(CardF1 F1)
        {
            string query = $"DELETE FROM TempCondition WHERE crimeId = {F1.CaseNumber}{F1.CrimeNumber}";
            ExecuteQuery(query);
            Console.WriteLine(query);
        }

        private void DeleteSceneInspectionInvolvedPerson(CardF1 F1)
        {
            string query = $"DELETE FROM SceneInspectionInvolvedPerson WHERE crimeId = {F1.CaseNumber}{F1.CrimeNumber}";
            ExecuteQuery(query);
            Console.WriteLine(query);
        }

        #endregion Обработка Ф-1

        #region Обработка Ф-1.1

        public void F11CardDataProccessing(StatisticCard card)
        {
            CardF1DotOne F11 = (CardF1DotOne)card;



            if (F11.Accounting != 9)
            {
                InsertFullCase(F11);
            }
            else
            {
                ChangeFullCase(F11);
            }
        }

        private void InsertFullCase(CardF1DotOne F11)
        {
            InsertIntoCaseProceedingStory_OnCases(F11);
            InsertCase(F11);
            SetInvestigationActions(F11);
            UpdateCaseStatus(F11);
            InsertActionInfo(F11);
            InsertCivilAction(F11);
            InsertExpertizes(F11);
            InsertSciTechMeansUse(F11);
        }

        private void ChangeFullCase(CardF1DotOne F11)
        {
            ChangeCase(F11);
            ChangeActionInfo(F11);
            ChangeCivilAction(F11);
            ChangeExpertizes(F11);
            ChangeSciTechMeansUse(F11);
        }

        private void InsertCase(CardF1DotOne F11)
        {
            string query = $"UPDATE Cases " +
                           $"SET UPKArticle = {F11.UPKArticle}, " +
                           $"UPKArticleDate = '{F11.UPKArticleDate}', " +
                           $"referralBody = '{F11.CaseSendTo}', " +
                           $"referralDate = '{F11.CaseSendDate}', " +
                           $"referralOutNum = '{F11.CaseSendOutNumber}', " +
                           $"connectedCaseNum = {F11.ConnectCase}, " +
                           $"DemandsTotal = {F11.DemandsTotal}, " +
                           $"DemandsAccepted = {F11.DemandsAccepted}, " +
                           $"DecisionAccept = {F11.DecisionAccept}, " +
                           $"investigatorPosition = '{KeyValueConvertor(F11.RegData.User.Position, "Positions")}', " +
                           $"investigatorRank = '{KeyValueConvertor(F11.RegData.User.Rank, "Ranks")}', " +
                           $"investigatorFIO = '{F11.RegData.User.FIO}' " +
                           $"WHERE number = {F11.CaseNumber}";

            ExecuteQuery(query);
        }

        private void InsertActionInfo(CardF1DotOne F11)
        {
            foreach (ActionInfo item in F11.ActionInfos)
            {
                string query = $"INSERT INTO ActionInfos(code, destination, date, caseNumber) VALUES({item.Code}, '{item.Destination}', '{item.Date}', {F11.CaseNumber})";
                ExecuteQuery(query);
            }
        }

        private void InsertCivilAction(CardF1DotOne F11)
        {
            foreach (CivilAction item in F11.CivilActions)
            {
                string query = $"INSERT INTO CivilActions(code, cost, caseNumber) VALUES({item.Code}, {item.Sum}, {F11.CaseNumber})";
                ExecuteQuery(query);
            }
        }

        private void InsertExpertizes(CardF1DotOne F11)
        {
            foreach (Expertize item in F11.Expertizes)
            {
                string query = $"INSERT INTO Expertizes(expertizeCode, instituteCode, count, cost, caseNumber) VALUES({item.ExpertizeCode}," +
                               $" {item.InstituteCode}, {item.Count}, {item.Cost}, {F11.CaseNumber})";
                ExecuteQuery(query);
            }
        }

        private void InsertSciTechMeansUse(CardF1DotOne F11)
        {
            foreach (SciTechMeanUse item in F11.SciTechMeanUses)
            {
                string query = $"INSERT INTO SciTechMeansUses(code, count, caseNumber) VALUES({item.Code}, {item.Count}, {F11.CaseNumber})";
                ExecuteQuery(query);
            }
        }

        private void DeleteActionInfo(CardF1DotOne F11)
        {
            string query = $"DELETE FROM ActionInfos WHERE caseNumber = {F11.CaseNumber}";
            ExecuteQuery(query);
        }

        private void DeleteCivilAction(CardF1DotOne F11)
        {
            string query = $"DELETE FROM CivilActions WHERE caseNumber = {F11.CaseNumber}";
            ExecuteQuery(query);
        }

        private void DeleteExpertizes(CardF1DotOne F11)
        {
            string query = $"DELETE FROM Expertizes WHERE caseNumber = {F11.CaseNumber}";
            ExecuteQuery(query);
        }

        private void DeleteSciTechMeansUse(CardF1DotOne F11)
        {
            string query = $"DELETE FROM SciTechMeansUses WHERE caseNumber = {F11.CaseNumber}";
            ExecuteQuery(query);
        }

        private void ChangeCase(CardF1DotOne F11)
        {
            foreach (ChangedField item in F11.ChangedFields)
            {
                string query = $"UPDATE Cases " +
                           $"SET {item.Field} = {item.Value} " +
                           $"WHERE number = {F11.CaseNumber}";

                ExecuteQuery(query);
            }
        }

        private void ChangeActionInfo(CardF1DotOne F11)
        {
            if (F11.ActionInfos.Count > 0)
            {
                DeleteActionInfo(F11);
                InsertActionInfo(F11);
            }
        }

        private void ChangeCivilAction(CardF1DotOne F11)
        {
            if (F11.CivilActions.Count > 0)
            {
                DeleteCivilAction(F11);
                InsertCivilAction(F11);
            }
        }

        private void ChangeExpertizes(CardF1DotOne F11)
        {
            if (F11.Expertizes.Count > 0)
            {
                DeleteExpertizes(F11);
                InsertExpertizes(F11);
            }
        }

        private void ChangeSciTechMeansUse(CardF1DotOne F11)
        {
            if (F11.SciTechMeanUses.Count > 0)
            {
                DeleteSciTechMeansUse(F11);
                InsertSciTechMeansUse(F11);
            }
        }

        private void InsertIntoCaseProceedingStory_OnCases(CardF1DotOne F11)
        {
            List<int?> stopCaseMarks = new List<int?>() { 2, 31, 3, 4, 7, 33, 8, 9, 12, 35, 13, 14, 15, 16, 17, 18, 19, 21, 36, 37, 38 };
            List<int?> finishedCaseMarks = new List<int?>() { 1, 32, 34, 30, 79 };

            int status = -1;

            if(stopCaseMarks.Contains(F11.UPKArticle))
            {
                status = 3;
            }
            if(finishedCaseMarks.Contains(F11.UPKArticle))
            {
                status = 7;
            }
            if(F11.UPKArticle == 26)
            {
                status = 5;
            }
            if (F11.UPKArticle == 28)
            {
                status = 4;
            }
            if (status != -1)
            {
                string query = $"INSERT INTO CaseProceedingStory(caseNumber, date, investigatorPosition, investigatorRank, investigatorFIO, status)" +
                            $"VALUES({F11.CaseNumber}, '{F11.UPKArticleDate}', '{KeyValueConvertor(F11.RegData.User.Position, "Positions")}', '{KeyValueConvertor(F11.RegData.User.Rank, "Ranks")}', '{F11.RegData.User.FIO}', {status})";
                ExecuteQuery(query);

            }
            
        }

        private void UpdateCaseStatus(CardF1DotOne F11)
        {
            List<int?> stopCaseMarks = new List<int?>() { 2, 31, 3, 4, 7, 33, 8, 9, 12, 35, 13, 14, 15, 16, 17, 18, 19, 21, 36, 37, 38 };

            int status = -1;

            if (stopCaseMarks.Contains(F11.UPKArticle))
            {
                status = 3;
            }
            if (F11.UPKArticle == 26)
            {
                status = 5;
            }
            if (F11.UPKArticle == 28)
            {
                status = 4;
            }

            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandText = $"SELECT StatusDate FROM Cases WHERE number = {F11.CaseNumber}";
            command.Transaction = transaction;
            DateTime? date = (DateTime?)command.ExecuteScalar();
            

            if (status != -1 && (F11.UPKArticleDate >= date || !date.HasValue))
            {
                string query = $"UPDATE Cases SET Status = {status} WHERE number = {F11.CaseNumber}";
                ExecuteQuery(query);
            }
        }

        private void SetInvestigationActions(CardF1DotOne F11)
        {

            string action = string.Empty;
            bool flag = false;

            List<int?> stopCaseMarks = new List<int?>() { 2, 31, 3, 4, 7, 33, 8, 9, 12, 35, 13, 14, 15, 16, 17, 18, 19, 21, 36, 37, 38 };

            if (stopCaseMarks.Contains(F11.UPKArticle))
            {
                action = KeyValueConvertor(F11.UPKArticle, "UPKarticle");
                flag = true;
            }
            if (F11.UPKArticle == 26)
            {
                action = $"Передано по подследственности в {F11.CaseSendTo} {F11.CaseSendDate.Value.ToShortDateString()}";
                flag = true;
            }


            string query = $"UPDATE WeeklyCaseTable SET investigationActions = '{action}' WHERE number = {F11.CaseNumber}";

            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            command.CommandText = $"SELECT StatusDate FROM Cases WHERE number = {F11.CaseNumber}";
            command.Transaction = transaction;
            DateTime? date = (DateTime?)command.ExecuteScalar();

            if ((F11.UPKArticleDate >= date || !date.HasValue) && flag) ExecuteQuery(query);

        }
        #endregion Обработка Ф-1.1

        #region Обработка Ф-1.2

        public void F12CardDataProccessing(StatisticCard card)
        {
            CardF1DotTwo F12 = (CardF1DotTwo)card;

            if (F12.Accounting != 9)
            {
                if (CheckUniqueField("SELECT id FROM Suspects", $"{F12.CaseNumber}{F12.SuspectNumber}", typeof(long)))
                {
                    Exception.Message = $"В базе данных уже имеется жулик с порядковым номером {F12.SuspectNumber.Value}";
                    Exception.HasError = true;
                    return;
                }

                InsertFullSuspect(F12);
            }
            else
            {
                ChangeFullSuspect(F12);
            }
        }

        private void InsertFullSuspect(CardF1DotTwo F12)
        {
            InsertSuspect(F12);
            InsertSuspectCrimes(F12);
            InsertPreventiveMeasures(F12);
            InsertOfficialStatus(F12);
        }

        private void InsertSuspect(CardF1DotTwo F12)
        {
            int? local;
            if (F12.Local) local = 50;
            else local = null;

            string query = $"INSERT INTO Suspects(id, caseNumberFK, number, militaryUnit, name, surname, patronymic, position, rank, local, individNumber, sex, birthday, age, birthLocality, birthArea, birthRegion, " +
                           $"liveLocality, liveArea, liveRegion, liveAddress, upbringing, familyStatus, children, RVKinstitute, instancePlace, callDate, education, citizenship, " +
                           $"nationality, socialStatus, country) " +
                           $"VALUES({F12.CaseNumber}{F12.SuspectNumber}, {F12.CaseNumber}, {F12.SuspectNumber}, '{F12.MilitaryUnit}', '{F12.FIO.Name}', '{F12.FIO.Surname}', '{F12.FIO.Patronymic}', '{F12.Position}', '{F12.Rank}', {local}, " +
                           $"'{F12.IndividNumber}', {F12.Sex}, '{F12.Birthday}', {F12.Age}, '{F12.BirthPlace.Locality}', '{F12.BirthPlace.Area}', '{F12.BirthPlace.Region}', " +
                           $"'{F12.LivePlace.Locality}', '{F12.LivePlace.Area}', '{F12.LivePlace.Region}', '{F12.LivePlace.Address}', {F12.Upbringing}, {F12.FamilyStatus}, " +
                           $"{F12.Children}, '{F12.RVKinstitute}', '{F12.InstancePlace}', '{F12.CallDate}', {F12.Education}, {F12.Citizenship}, '{F12.Nationality}', {F12.SocialStatus}, " +
                           $"'{F12.Country}')";
            ExecuteQuery(query);
        }

        private void InsertSuspectCrimes(CardF1DotTwo F12)
        {
            foreach (Crime crime in F12.Crimes)
            {
                string query = $"INSERT INTO SuspectsCrimesPivot(suspectId, crimeId, suspectNumber, complicity, state, article, sign, part, paragraph) " +
                               $"VALUES ({F12.CaseNumber}{F12.SuspectNumber}, {F12.CaseNumber}{crime.CrimeNumber}, {F12.SuspectNumber}, {crime.Complicity}, {crime.Situation}, " +
                               $"{crime.Qualification.Article}, '{crime.Qualification.Sign}', '{crime.Qualification.Part}', '{crime.Qualification.Paragraph}')";
                ExecuteQuery(query);
            }
        }

        private void InsertPreventiveMeasures(CardF1DotTwo F12)
        {
            string query = $"INSERT INTO PreventiveMeasures(caseNumberFK, suspectID, coerciveMeasure, coerciveMeasureDate, arrestExtension, arrestExtensionEnd, creationDate) " +
                           $"VALUES({F12.CaseNumber}, {F12.CaseNumber}{F12.SuspectNumber}, {F12.CoersiveMeasure}, '{F12.CoersiveMeasureDate}', " +
                           $"{F12.ArrestDuration}, '{F12.ArrestTimeEnd}', {F12.CardCreationDate.ToFileTime()})";
            ExecuteQuery(query);
        }

        private void InsertOfficialStatus(CardF1DotTwo F12)
        {
            foreach (El item in F12.OfficialStatus)
            {
                string query = $"INSERT INTO OfficialStatus(code, SuspectId) VALUES ({item.Num}, {F12.CaseNumber}{F12.SuspectNumber})";
                ExecuteQuery(query);
            }
        }

        private void DeleteOfficialStatus(CardF1DotTwo F12)
        {
            string query = $"DELETE FROM OfficialStatus WHERE SuspectId = {F12.CaseNumber}{F12.SuspectNumber}";
            ExecuteQuery(query);
        }

        private void ChangeOfficialStatus(CardF1DotTwo F12)
        {
            if (F12.OfficialStatus.Count > 0) DeleteOfficialStatus(F12);
            InsertOfficialStatus(F12);
        }

        private void ChangeFullSuspect(CardF1DotTwo F12)
        {
            ChangeSuspect(F12);
            ChangeSuspectCrimes(F12);
            ChangeOfficialStatus(F12);
        }

        private void ChangeSuspect(CardF1DotTwo F12)
        {
            foreach (ChangedField item in F12.ChangedFields)
            {
                string query;
                if (item.Table == "Suspects")
                {
                    query = $"UPDATE Suspects " +
                           $"SET {item.Field} = {item.Value} " +
                           $"WHERE id = {F12.CaseNumber}{F12.SuspectNumber}";
                }
                else
                {
                    query = $"UPDATE PreventiveMeasures " +
                           $"SET {item.Field} = {item.Value} " +
                           $"WHERE creationDate = {F12.CardCreationDate.ToFileTime()}";
                }

                Console.WriteLine(query);
                ExecuteQuery(query);
            }
        }

        private void ChangeSuspectCrimes(CardF1DotTwo F12)
        {
            if (F12.Crimes.Count > 0) DeleteSuspectCrimes(F12);
            InsertSuspectCrimes(F12);
        }

        private void DeleteSuspectCrimes(CardF1DotTwo F12)
        {
            string query = $"DELETE FROM SuspectsCrimesPivot WHERE suspectId = {F12.CaseNumber}{F12.SuspectNumber}";

            ExecuteQuery(query);
        }

        #endregion Обработка Ф-1.2

        #region Обработка Ф-1.3

        public void F13CardDataProccessing(StatisticCard card)
        {
            CardF1DotThree F13 = (CardF1DotThree)card;

            if (F13.Accounting != 9)
            {
                InsertPreventiveMeasures(F13);
            }
            else
            {
                ChangePreventiveMeasures(F13);
            }
        }

        private void InsertPreventiveMeasures(CardF1DotThree F13)
        {
            foreach (OnSuspectMeasure item in F13.Measures)
            {
                string query = $"INSERT INTO PreventiveMeasures(caseNumberFK, suspectID, coerciveMeasure, coerciveMeasureDate, arrestExtension, arrestExtensionEnd, creationDate) " +
                           $"VALUES({F13.CaseNumber}, {F13.CaseNumber}{item.SuspectNumber}, {item.CoerciveMeasureCode}, '{item.CoerciveMeasureDateStart}', " +
                           $"{item.MeasureDuration}, '{item.CoerciveMeasureDateEnd}', {F13.CardCreationDate.ToFileTime()})";
                ExecuteQuery(query);
            }
        }

        private void DeletePreventiveMeasures(CardF1DotThree F13)
        {
            string query = $"DELETE FROM PreventiveMeasures WHERE creationDate = {F13.CardCreationDate.ToFileTime()}";
            ExecuteQuery(query);
        }

        private void ChangePreventiveMeasures(CardF1DotThree F13)
        {
            if (F13.Measures.Count > 0) DeletePreventiveMeasures(F13);
            InsertPreventiveMeasures(F13);
        }

        #endregion Обработка Ф-1.3

        #region Обработка Ф-2

        public void F2CardDataProccessing(StatisticCard card)
        {
            CardF2 F2 = (CardF2)card;

            if (F2.Accounting != 9)
            {
                InsertFullSuspect(F2);
            }
            else
            {
                ChangeFullSuspect(F2);
            }
        }

        private void InsertFullSuspect(CardF2 F2)
        {
            InsertSuspect(F2);
            ChangeSuspectCrimes(F2);
            InsertPreventiveMeasures(F2);
        }

        private void InsertSuspect(CardF2 F2)
        {
            string query = $"UPDATE Suspects " +
                           $"SET criminlCount = {F2.CriminalCount}, " +
                           $"pastArticles = '{F2.PastArticles}', " +
                           $"conviction = {F2.Conviction}, " +
                           $"crimeRelapse = {F2.CrimeRelapse}, " +
                           $"earlyCases = {F2.EarlyCases}, " +
                           $"personState = {F2.PersonState}, " +
                           $"personSituation = {F2.PersonSituation}, " +
                           $"onPersonDecision = {F2.OnPersonDecision}, " +
                           $"caseStopReason = {F2.CaseStopReason}," +
                           $"militaryUnit = '{F2.MilitaryUnit}', " +
                           $"ownArrest = {F2.OwnArrest} " +
                           $"WHERE id = {F2.CaseNumber}{F2.SuspectNumber}";
            ExecuteQuery(query);
        }

        private void InsertPreventiveMeasures(CardF2 F2)
        {
            string query = $"INSERT INTO PreventiveMeasures(caseNumberFK, suspectID, coerciveMeasure, coerciveMeasureDate, creationDate) " +
                           $"VALUES({F2.CaseNumber}, {F2.CaseNumber}{F2.SuspectNumber}, {F2.CoerciveMeasureCode}, '{F2.CoerciveMeasureDate}', {F2.CardCreationDate.ToFileTime()})";
            ExecuteQuery(query);
        }

        private void InsertSuspectCrimes(CardF2 F2)
        {
            foreach (Crime crime in F2.Crimes)
            {
                string query = $"INSERT INTO SuspectsCrimesPivot(suspectId, crimeId, suspectNumber, complicity, state, judicialFine, article, sign, part, paragraph) " +
                               $"VALUES ({F2.CaseNumber}{F2.SuspectNumber}, {F2.CaseNumber}{crime.CrimeNumber}, {F2.SuspectNumber}, {crime.Complicity}, {crime.Situation}, " +
                               $"{crime.JudicialFine}, {crime.Qualification.Article}, '{crime.Qualification.Sign}', '{crime.Qualification.Part}', '{crime.Qualification.Paragraph}')";
                ExecuteQuery(query);
            }
        }

        private void ChangeFullSuspect(CardF2 F2)
        {
            ChangeSuspect(F2);
            ChangeSuspectCrimes(F2);
        }

        private void ChangeSuspect(CardF2 F2)
        {
            foreach (ChangedField item in F2.ChangedFields)
            {
                string query;
                if (item.Table == "Suspects")
                {
                    query = $"UPDATE Suspects " +
                           $"SET {item.Field} = {item.Value} " +
                           $"WHERE id = {F2.CaseNumber}{F2.SuspectNumber}";
                }
                else
                {
                    query = $"UPDATE PreventiveMeasures " +
                           $"SET {item.Field} = {item.Value} " +
                           $"WHERE creationDate = {F2.CardCreationDate.ToFileTime()}";
                }
                ExecuteQuery(query);
            }
        }

        private void ChangeSuspectCrimes(CardF2 F2)
        {
            if (F2.Crimes.Count > 0) DeleteSuspectCrimes(F2);
            InsertSuspectCrimes(F2);
        }

        private void DeleteSuspectCrimes(CardF2 F2)
        {
            string query = $"DELETE FROM SuspectsCrimesPivot WHERE suspectId = {F2.CaseNumber}{F2.SuspectNumber}";

            ExecuteQuery(query);
        }

        #endregion Обработка Ф-2

        #region Обработка Ф-3

        public void F3CardDataProccessing(StatisticCard card)
        {
            CardF3 F3 = (CardF3)card;

            InsertCaseMovement(F3);
            SetInvestigationActions(F3);
        }

        private void InsertCaseMovement(CardF3 F3)
        {
            foreach (CaseDecision item in F3.CaseDecisions)
            {
                string query = $"INSERT INTO CriminalCaseMovement(caseNumber, CaseDecision, DecisionDate, DecisionSource, termExtension, " +
                               $"termExtensionDate, CaseExtendReason, DemandsTotal, DemandsAccepted, creationDate, investigatorPosition, investigatorRank, investigatorFIO) " +
                               $"VALUES({F3.CaseNumber}, {item.Decision}, '{item.Date}', {item.Source}, {item.TermExtension}, '{item.TermExtensionDate}', " +
                               $"{item.CaseExtendReason}, {F3.DemandsTotal}, {F3.DemandsAccepted}, {F3.CardCreationDate.ToFileTime()}, " +
                               $"'{KeyValueConvertor(F3.RegData.User.Position, "Positions")}', '{KeyValueConvertor(F3.RegData.User.Rank, "Ranks")}', '{F3.RegData.User.FIO}')";
                ExecuteQuery(query);
            }
        }

        private void SetInvestigationActions(CardF3 F3)
        {
            foreach (CaseDecision item in F3.CaseDecisions)
            {
                string action = string.Empty;
                bool flag = false;

                if (item.Decision == 22) { action =  $"Дело приостановлено {item.Date.Value.ToShortDateString()} на основании п. 2 ч. 1 ст. 208 УПК РФ"; flag = true;}
                if (item.Decision == 24) { action =  $"Дело приостановлено {item.Date.Value.ToShortDateString()} на основании п. 1 ч. 1 ст. 208 УПК РФ"; flag = true;}
                if (item.Decision == 23) { action =  $"Дело приостановлено {item.Date.Value.ToShortDateString()} на основании п. 4 ч. 1 ст. 208 УПК РФ"; flag = true;}
                if (item.Decision == 29) { action =  $"Дело приостановлено {item.Date.Value.ToShortDateString()} на основании п. 3 ч. 1 ст. 208 УПК РФ"; flag = true;}
                if (item.Decision == 72) { action =  $"Направлено в ВП ВГ {item.Date}"; flag = true; }

                string query = $"UPDATE WeeklyCaseTable SET investigationActions = '{action}' WHERE number = {F3.CaseNumber}";

                SqlCommand command = new SqlCommand();
                command.Connection = Connection;
                command.CommandText = $"SELECT StatusDate FROM Cases WHERE number = {F3.CaseNumber}";
                command.Transaction = transaction;

                var result = command.ExecuteScalar();
                DateTime? date;
 
                date = result != DBNull.Value ? (DateTime?)result : null;

                if ((item.Date >= date || !date.HasValue) && flag ) ExecuteQuery(query);
            }
        }
        #endregion Обработка Ф-3

        #region Обработка Ф-5

        public void F5CardDataProccessing(StatisticCard card)
        {
            CardF5 F5 = (CardF5)card;

            if (F5.Accounting != 9)
            {
                if (CheckUniqueField("SELECT id FROM VictimIndividual", $"{F5.CaseNumber}{F5.VictimNumber}", typeof(long)))
                {
                    Exception.Message = $"В базе данных уже имеется преступление с номером {F5.VictimNumber.Value}";
                    Exception.HasError = true;
                    return;
                }

                InsertFullVictim(F5);
            }
            else
            {
                ChangeFullVictim(F5);
            }
        }

        private void InsertFullVictim(CardF5 F5)
        {
            InsertVictim(F5);
            InsertVictimCrime(F5);
            InsertOfficialStatus(F5);
        }

        private void InsertVictim(CardF5 F5)
        {
            int? local = null;
            if (F5.Local) local = 50;
            string query = $"INSERT INTO VictimIndividual(id, number, militaryUnit, name, surname, patronymic, position, rank, local, individNumber, sex, birthday, age, birthLocality, birthArea, birthRegion, " +
                           $"liveLocality, liveArea, liveRegion, liveAddress, upbringing, familyStatus, children, RVKInstitute, instancePlace, callDate, education, citizenship, nationality, " +
                           $"socialStatus, provocativeActions, crimePersonsRelation, country) " +
                           $"VALUES({F5.CaseNumber}{F5.VictimNumber}, {F5.VictimNumber}, '{F5.MilitaryUnit}', '{F5.FIO.Name}', '{F5.FIO.Surname}', '{F5.FIO.Patronymic}', '{F5.Position}', '{F5.Rank}', {local}, " +
                           $"'{F5.IndividNumber}', {F5.Sex}, '{F5.Birthday}', {F5.Age}, '{F5.BirthPlace.Locality}', '{F5.BirthPlace.Area}', '{F5.BirthPlace.Region}', '{F5.LivePlace.Locality}', " +
                           $"'{F5.LivePlace.Area}', '{F5.LivePlace.Region}', '{F5.LivePlace.Address}', {F5.Upbringing}, {F5.FamilyStatus}, {F5.Children}, '{F5.RVKinstitute}', '{F5.InstancePlace}', " +
                           $"'{F5.CallDate}', {F5.Education}, {F5.Citizenship}, '{F5.Nationality}', {F5.SocialStatus}, {F5.ProvocativeActions}, {F5.CrimePersonsRelation}, '{F5.Country}')";
            ExecuteQuery(query);
        }

        private void InsertVictimCrime(CardF5 F5)
        {
            foreach (VictimCrime item in F5.VictimCrimes)
            {
                string query = $"INSERT INTO VictimCrimePivot(crimeID, individualId, state, aftermath, article, sign, part, paragraph) " +
                               $"VALUES({F5.CaseNumber}{item.CrimeNumber}, {F5.CaseNumber}{F5.VictimNumber}, {item.Condition}, {item.ConsequencesNature}, " +
                               $"{item.Qualification.Article}, '{item.Qualification.Sign}', '{item.Qualification.Part}', '{item.Qualification.Paragraph}')";
                ExecuteQuery(query);
            }
        }

        private void ChangeFullVictim(CardF5 F5)
        {
            ChangeVictim(F5);
            ChangeVictimCrime(F5);
            ChangeOfficialStatus(F5);
        }

        private void ChangeVictim(CardF5 F5)
        {
            foreach (ChangedField item in F5.ChangedFields)
            {
                string query = $"UPDATE VictimIndividual " +
                               $"SET {item.Field} = {item.Value} " +
                               $"WHERE id = {F5.CaseNumber}{F5.VictimNumber}";
                ExecuteQuery(query);
            }
        }

        private void DeleteVictimCrime(CardF5 F5)
        {
            string query = $"DELETE FROM VictimCrimePivot WHERE individualId = {F5.CaseNumber}{F5.VictimNumber}";
            ExecuteQuery(query);
        }

        private void ChangeVictimCrime(CardF5 F5)
        {
            if (F5.VictimCrimes.Count > 0) DeleteVictimCrime(F5);
            InsertVictimCrime(F5);
        }

        private void InsertOfficialStatus(CardF5 F5)
        {
            foreach (El item in F5.OfficialStatus)
            {
                string query = $"INSERT INTO OfficialStatus(code, VictimId) VALUES ({item.Num}, {F5.CaseNumber}{F5.VictimNumber})";
                ExecuteQuery(query);
            }
        }

        private void DeleteOfficialStatus(CardF5 F5)
        {
            string query = $"DELETE FROM OfficialStatus WHERE VictimId = {F5.CaseNumber}{F5.VictimNumber}";
            ExecuteQuery(query);
        }

        private void ChangeOfficialStatus(CardF5 F5)
        {
            if (F5.OfficialStatus.Count > 0) DeleteOfficialStatus(F5);
            InsertOfficialStatus(F5);
        }

        #endregion Обработка Ф-5

        #region Обработка Ф-5.5

        public void F55CardDataProccessing(StatisticCard card)
        {
            CardF5DotFive F55 = (CardF5DotFive)card;

            if (F55.Accounting != 9)
            {
                if (CheckUniqueField("SELECT number FROM VictimEntity", $"{F55.CaseNumber}{F55.VictimNumber}", typeof(long)))
                {
                    Exception.Message = $"В базе данных уже имеется преступление с номером {F55.VictimNumber.Value}";
                    Exception.HasError = true;
                    return;
                }

                InsertFullVictim(F55);
            }
            else
            {
                ChangeFullVictim(F55);
            }
        }

        private void InsertFullVictim(CardF5DotFive F55)
        {
            InsertVictim(F55);
            InsertVictimCrime(F55);
        }

        private void InsertVictim(CardF5DotFive F55)
        {
            string query = $"INSERT INTO VictimEntity(id, number, name, ownForm, Locality, Area, Region) " +
                           $"VALUES({F55.CaseNumber}{F55.VictimNumber}, {F55.VictimNumber}, '{F55.OrganizationName}', {F55.OwnForm}, '{F55.OrgPlace.Locality}', '{F55.OrgPlace.Area}', '{F55.OrgPlace.Region}')";
            ExecuteQuery(query);
        }

        private void InsertVictimCrime(CardF5DotFive F55)
        {
            foreach (OwnCrime item in F55.OwnCrimes)
            {
                string query = $"INSERT INTO VictimCrimePivot(crimeId, entityId, aftermath, materialDamage, article, sign, part, paragraph) " +
                               $"VALUES({F55.CaseNumber}{item.CrimeNumber}, {F55.CaseNumber}{F55.VictimNumber}, {item.ConsequencesNature}, {item.DamageSum}, " +
                               $"{item.Qualification.Article}, '{item.Qualification.Sign}', '{item.Qualification.Part}', '{item.Qualification.Paragraph}')";
                ExecuteQuery(query);
            }
        }

        private void ChangeFullVictim(CardF5DotFive F55)
        {
            ChangeVictim(F55);
            ChangeVictimCrime(F55);
        }

        private void ChangeVictim(CardF5DotFive F55)
        {
            foreach (ChangedField item in F55.ChangedFields)
            {
                string query = $"UPDATE VictimEntity " +
                               $"SET {item.Field} = {item.Value} " +
                               $"WHERE id = {F55.CaseNumber}{F55.VictimNumber}";
                ExecuteQuery(query);
            }
        }

        private void ChangeVictimCrime(CardF5DotFive F55)
        {
            if (F55.OwnCrimes.Count > 0) DeleteVictimCrime(F55);
            InsertVictimCrime(F55);
        }

        private void DeleteVictimCrime(CardF5DotFive F55)
        {
            string query = $"DELETE FROM VictimCrimePivot WHERE entityId = {F55.CaseNumber}{F55.VictimNumber}";
            ExecuteQuery(query);
        }

        #endregion Обработка Ф-5.5

        public SqlConnection Connection { get; set; }
        private string connectionString;
        public Dictionary<Type, FCardDataProccessing> cardProccessor = new Dictionary<Type, FCardDataProccessing>();
        private SqlTransaction transaction;
        private SqlCommand command;

        public CriminalCaseDB()
        {
            Exception = new ServerException();

            cardProccessor.Add(typeof(CardF1), F1CardDataProccessing);
            cardProccessor.Add(typeof(CardF1DotOne), F11CardDataProccessing);
            cardProccessor.Add(typeof(CardF1DotTwo), F12CardDataProccessing);
            cardProccessor.Add(typeof(CardF1DotThree), F13CardDataProccessing);
            cardProccessor.Add(typeof(CardF2), F2CardDataProccessing);
            cardProccessor.Add(typeof(CardF3), F3CardDataProccessing);
            cardProccessor.Add(typeof(CardF5), F5CardDataProccessing);
            cardProccessor.Add(typeof(CardF5DotFive), F55CardDataProccessing);

            string fileserver = StatCardApp.Function.StatCardFunc.GetFileText(@".\ServerConfiguration.conf");
            ServerConfig config = (ServerConfig)JsonConvert.DeserializeObject(fileserver, typeof(ServerConfig));
            connectionString = config.Connection;
        }

        public void Connect()
        {
            Connection = new SqlConnection(connectionString);

            try
            {
                // Открываем подключение
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                Console.WriteLine("Подключение открыто");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InsertCard(StatisticCard card)
        {
            transaction = Connection.BeginTransaction();

            command = Connection.CreateCommand();
            command.Transaction = transaction;

            try
            {
                if (!CaseNumberExist(card))
                {
                    transaction.Commit();
                    return;
                }
                cardProccessor[card.TypeOfCard].Invoke(card);
                transaction.Commit();
            }
            catch (Exception e)
            {
                Exception.Message = e.Message;
                Exception.HasError = true;
                transaction.Rollback();
            }
        }

        private void CreateWeeklyCaseTableRow(CaseInfoModel caseInfo)
        {
            try
            {
                ExecuteQuery($"INSERT WeeklyCaseTable (number) VALUES ({caseInfo.FullCaseNumber})");
            }
            catch(SqlException e)
            {
                Exception.Message = e.Message; 
                Exception.HasError = true;
            }
        }

        public void InsertCaseProduction(CaseProductionInfo productionInfo)
        {
            try
            {
                ExecuteQuery($"INSERT CaseProductionStory (caseNumber, date, investigatorPosition, investigatorRank, investigatorFIO) VALUES ({productionInfo.CaseNumber}, '{productionInfo.Date}', '{productionInfo.Position}', '{productionInfo.Rank}', '{productionInfo.FIO}')");
            }
            catch (SqlException e)
            {
                Exception.Message = e.Message;
                Exception.HasError = true;
            }
        }

        public void InsertCase(CaseInfoModel caseInfo)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = this.Connection;
            command.CommandText = $"INSERT Cases(number) VALUES ({caseInfo.FullCaseNumber})";

            try
            {
                int result = command.ExecuteNonQuery();
                Console.WriteLine($"Вставлено {result} строк");
                CreateWeeklyCaseTableRow(caseInfo);
            }
            catch (SqlException e)
            {
                if (e.Number == 2627)
                {
                    Exception.Message = $"Уголовное дело № {caseInfo.FullCaseNumber} уже существует в базе";
                    Exception.HasError = true;
                }
                else
                {
                    Exception.Message = e.Message;
                    Exception.HasError = true;
                }
            }
        }

        private void ExecuteQuery(string query)
        {
            query = SetNullValues(query);
            Console.WriteLine(query);
            command.CommandText = query;
            int result = command.ExecuteNonQuery();
        }

        private string SetNullValues(string queryData)
        {
            queryData = queryData.Replace(" ,", " null,");
            queryData = queryData.Replace(" )", "null)");
            queryData = queryData.Replace("( ", "(null");
            queryData = queryData.Replace("= ,", "= null");
            queryData = queryData.Replace("=  WHERE", "= null WHERE");
            return queryData;
        }

        private bool CheckUniqueField(string uniqueFieldsListQuery, object fieldToCheck, Type type)
        {
            var key = fieldToCheck == null ? (DateTime?)null : Convert.ChangeType(fieldToCheck, type);

            command.CommandText = uniqueFieldsListQuery;
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    try
                    {
                        if (key.Equals(Convert.ChangeType(reader.GetValue(0), type)))
                        {
                            reader.Close();
                            return true;
                        }
                    }
                    catch 
                    {
                        reader.Close();
                        return false;
                    }

                }
            }
            reader.Close();
            return false;
        }

        private bool CaseNumberExist(StatisticCard card)
        {
            if (!CheckUniqueField("SELECT number FROM Cases", card.CaseNumber, card.CaseNumber.GetType()))
            {
                Exception.Message = $"В базе данных отсутствует уголовное дело с номером {card.CaseNumber}";
                Exception.HasError = true;
                return false;
            }
            else
            {
                return true;
            }
        }

        private string KeyValueConvertor(object value, object parameter)
        {
            PropertyInfo prop = typeof(ListsData).GetProperty((string)parameter);
            List<El> list = (List<El>)prop.GetValue(null);
            string strFieldValue = (from el in list where el.Num == System.Convert.ToInt32(value) select el.Str).Single();
            return strFieldValue;
        }

        public void ExecuteNonQuery(string commandText)
        {
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader(string commandText)
        {
            command.CommandText = commandText;
            return command.ExecuteReader();
        }

        private List<CriminalProceedingsInfo> GetCriminalProceedingsInfo(string caseNumber)
        {
            CriminalProceedingsInfo criminalProceedings;
            List<CriminalProceedingsInfo> criminalProceedingsList = new List<CriminalProceedingsInfo>();

            SqlDataReader proceedingReader = ExecuteReader($"SELECT * FROM CaseProceedingStory WHERE caseNumber = {caseNumber} ORDER BY date");
            if (proceedingReader.HasRows)
            {
                while (proceedingReader.Read())
                {
                    criminalProceedings = new CriminalProceedingsInfo();

                    criminalProceedings.Status = Convert.ToInt32(proceedingReader["status"]);
                    criminalProceedings.Date = Convert.ToDateTime(proceedingReader["date"]).ToShortDateString();
                    criminalProceedings.FIO = proceedingReader["investigatorFIO"].ToString();
                    criminalProceedings.Position = proceedingReader["investigatorPosition"].ToString();
                    criminalProceedings.Rank = proceedingReader["investigatorRank"].ToString();
                    criminalProceedings.Qualification = proceedingReader["qualification"].ToString();

                    criminalProceedingsList.Add(criminalProceedings);
                }
            }
            proceedingReader.Close();

            return criminalProceedingsList;
        }

        private string GetCriminalProceedingsInfoString(List<CriminalProceedingsInfo> criminalProceedingList)
        {
            string result = string.Empty;
            foreach (CriminalProceedingsInfo item in criminalProceedingList)
            {
                result += $"{item.StringView}   ";
            }

            return result;
        }

        private List<CriminalProductionInfo> GetCriminalProductionInfo(string caseNumber)
        {
            CriminalProductionInfo criminalProduction;
            List<CriminalProductionInfo> criminalProductionList = new List<CriminalProductionInfo>();

            SqlCommand command = new SqlCommand("GET_CASEPRODUCTIONS", Connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter caseNumberParameter = new SqlParameter
            {
                ParameterName = "@caseNumber",
                Value = caseNumber
            };
            command.Parameters.Add(caseNumberParameter);
            SqlDataReader productionReader = command.ExecuteReader();

            if (productionReader.HasRows)
            {
                while (productionReader.Read())
                {
                    criminalProduction = new CriminalProductionInfo();

                    criminalProduction.Date = Convert.ToDateTime(productionReader["date"]).ToShortDateString();
                    criminalProduction.FIO = productionReader["investigatorFIO"].ToString();
                    criminalProduction.Position = productionReader["investigatorPosition"].ToString();
                    criminalProduction.Rank = productionReader["investigatorRank"].ToString();

                    criminalProductionList.Add(criminalProduction);
                }
            }
            productionReader.Close();

            return criminalProductionList;
        }

        private string GetCriminalProductionInfoString(List<CriminalProductionInfo> criminalProductionList)
        {
            string result = string.Empty;
            foreach (CriminalProductionInfo item in criminalProductionList)
            {
                result += $"{item.StringView}   ";
            }

            return result;
        }

        private List<SuspectInfo> GetSuspectInfo(string caseNumber)
        {
            SuspectInfo suspectInfo;
            List<SuspectInfo> suspectInfoList = new List<SuspectInfo>();

            SqlDataReader suspectReader = ExecuteReader($"SELECT surname + ' ' + LEFT(name, 1) + '. ' + LEFT(patronymic, 1) + '.' as fio, militaryUnit, rank FROM Suspects WHERE LEFT(CAST(id as varchar(20)), 17) = {caseNumber}");

            if (suspectReader.HasRows)
            {
                while (suspectReader.Read())
                {
                    suspectInfo = new SuspectInfo();

                    suspectInfo.FIO = suspectReader["fio"].ToString();
                    suspectInfo.ServicePlace = suspectReader["militaryUnit"].ToString();
                    suspectInfo.Rank = suspectReader["rank"].ToString();

                    suspectInfoList.Add(suspectInfo);
                }
            }
            suspectReader.Close();

            return suspectInfoList;
        }

        private string GetSuspectInfoString(List<SuspectInfo> suspectInfoList)
        {
            string result = string.Empty;
            foreach (SuspectInfo item in suspectInfoList)
            {
                result += $"{item.StringView}   ";
            }

            return result;
        }

        private string GetCrimeInfo(string caseNumber)
        {
            string result = string.Empty;
            SqlDataReader reader = ExecuteReader($"SELECT fable FROM WeeklyCaseTable WHERE number = {caseNumber}");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = reader["fable"].ToString();
                }
            }
            reader.Close();
            return result;
        }

        private string GetInvestigationActions(string caseNumber)
        {
            string result = string.Empty;
            SqlDataReader reader = ExecuteReader($"SELECT investigationActions FROM WeeklyCaseTable WHERE number = {caseNumber}");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result = reader["investigationActions"].ToString();
                }
            }
            reader.Close();
            return result;
        }

        public void InsertInfoIntoDataBase(List<CaseDescriptionStatus> list)
        {
            foreach (CaseDescriptionStatus item in list)
            {
                string sqlExpression = $"UPDATE WeeklyCaseTable " +
                                   $"SET fable = '{item.CrimeInfo}', investigationActions = '{item.InvestigationActions}' " +
                                   $"WHERE number = {item.CaseNumber}";
                ExecuteNonQuery(sqlExpression);
            }
        }
        private int GetCaseStatus(string caseNumber)
        {
            int status = -1;
            SqlDataReader reader = ExecuteReader($"SELECT Status FROM Cases WHERE number = {caseNumber}");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    status = Convert.ToInt32(reader["Status"]);
                }
            }
            reader.Close();
            return status;
        }

        private int GetInvestigationTerm(string caseNumber)
        {
            int term = -1;
            SqlDataReader reader = ExecuteReader($"SELECT InvestigationTerm FROM Cases WHERE number = {caseNumber}");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    term = Convert.ToInt32(reader["InvestigationTerm"]);
                }
            }
            reader.Close();
            return term;
        }

        private DateTime GetInvestigationEnd(string caseNumber)
        {
            DateTime term = DateTime.Today;
            SqlDataReader reader = ExecuteReader($"SELECT InvestigationEnd FROM Cases WHERE number = {caseNumber}");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    term = Convert.ToDateTime(reader["InvestigationEnd"]);
                }
            }
            reader.Close();
            return term;
        }

        private string GetGuardTerm(string caseNumber)
        {
            string result = string.Empty;

            SqlDataReader reader = ExecuteReader($"SELECT surname + ' ' + LEFT(name, 1) + '. ' + LEFT(patronymic, 1) + '.' as fio, GuardTerm, GuardEnd FROM Suspects WHERE caseNumberFK = {caseNumber}");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string tempResult = string.Empty;

                    tempResult += reader["fio"].ToString() + "\n";

                    if (reader["GuardTerm"] != DBNull.Value)
                        tempResult += reader["GuardTerm"].ToString() + " месяца\n";
                    else
                        continue;

                    if (reader["GuardEnd"] != DBNull.Value)
                        tempResult += Convert.ToDateTime(reader["GuardEnd"]).ToShortDateString() + "\n";
                    else
                        continue;

                    result += $"{tempResult}\n";
                }
            }
            reader.Close();
            return result;
        }

        public List<CaseDescriptionStatus> GetCasesList()
        {
            List<CaseDescriptionStatus> CasesList = new List<CaseDescriptionStatus>();
            List<string> CaseNumbers = new List<string>();

            string sqlExpression = "SELECT number FROM Cases WHERE Status in (1, 2, 6) UNION SELECT number FROM Cases WHERE Status in (3, 4, 5, 7, 8) AND StatusDate < GETDATE() AND StatusDate > DATEADD(week, -1, GETDATE())";

            command.CommandText = sqlExpression;
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string number = reader["number"].ToString();
                    CaseNumbers.Add(number);
                }
            }
            reader.Close();
            int identificator = 1;
            foreach (string caseNumber in CaseNumbers)
            {

                CaseDescriptionStatus caseDescription = new CaseDescriptionStatus();
                caseDescription.Status = GetCaseStatus(caseNumber);
                caseDescription.pNum = identificator;
                caseDescription.CaseNumber = caseNumber;
                caseDescription.CriminalProceedings = GetCriminalProceedingsInfoString(GetCriminalProceedingsInfo(caseNumber));
                caseDescription.CriminalProductions = GetCriminalProductionInfoString(GetCriminalProductionInfo(caseNumber));
                caseDescription.Suspects = GetSuspectInfoString(GetSuspectInfo(caseNumber));
                caseDescription.CrimeInfo = GetCrimeInfo(caseNumber);
                caseDescription.InvestigationActions = GetInvestigationActions(caseNumber);
                caseDescription.GuardTerm = GetGuardTerm(caseNumber);

                int term = GetInvestigationTerm(caseNumber);
                DateTime end = GetInvestigationEnd(caseNumber);
                if (term == 0)
                    caseDescription.InvestigationTerm = $"15 суток\n {end.ToShortDateString()}";
                if (term == 1)
                    caseDescription.InvestigationTerm = $"30 суток\n {end.ToShortDateString()}";
                if (term >= 2)
                    caseDescription.InvestigationTerm = $"{term} месяца\n {end.ToShortDateString()}";

                if (term > 2 && (caseDescription.Status == 1 || caseDescription.Status == 4 || caseDescription.Status == 6))
                    caseDescription.Status = 0;

                identificator++;

                CasesList.Add(caseDescription);
            }

            List<CaseDescriptionStatus> SortedCasesList = SortCasesList(CasesList);

            return SortedCasesList;
        }

        private List<CaseDescriptionStatus> SortCasesList(List<CaseDescriptionStatus> CasesList)
        {
            List<int> sortedStatus = new List<int>() { 0, 1, 4, 6, 2, 3, 7, 5, 8 };

            List<CaseDescriptionStatus> SortedCasesList = new List<CaseDescriptionStatus>();

            foreach (int status in sortedStatus)
            {
                foreach (CaseDescriptionStatus caseDesc in CasesList)
                {
                    if (caseDesc.Status == status)
                    {
                        SortedCasesList.Add(caseDesc);
                        continue;
                    }
                }
            }

            return SortedCasesList;
        }
    }
}