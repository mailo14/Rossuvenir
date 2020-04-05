using KvotaWeb.Models;
using KvotaWeb.Models.Items;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KvotaWeb
{
    public static class Importer
    {
        public static bool IsCompare = false;
        //= true;
        internal static object Import(string filename = @"C:\Downloads\РАБОТА @\Калькулятор КВОТА 2.0\Rossuvenir\калькулятор 5 - блокноты, деколь\калькулятор 2020.04.01 — копия.xlsx")
        {
            FileStream inputStream = new FileStream(filename, FileMode.Open);
            return Import(inputStream);
        }

        public static string Import(Stream inputStream)
        {
            kvotaEntities db = new kvotaEntities();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(inputStream);
                var mySheets = workbook.Worksheets;

                try
                {
                    foreach (var mySheet in mySheets)
                    {
                        var cell = mySheet.Range;
                        switch (GetInt(cell[1, 1]))
                        {
                            case (int)TipProds.DTG:
                                ReadSingleRowTable(db, 415, 2, 3, cell);
                                break;
                            case (int)TipProds.Gravirovka:
                                ReadParam(db, 3, 3, cell);
                                ReadParam(db, 4, 3, cell);
                                ReadParam(db, 6, 3, cell);
                                break;
                            case (int)TipProds.Decol:
                                Read2DTable(db, 307, 3, 3, cell);
                                ReadParam(db, 9, 3, cell);

                                Read2DTable(db, 308, 12, 3, cell);
                                break;
                            case (int)TipProds.Tampopechat:
                                Read2DTable(db, 42, 3, 3, cell);
                                Read2DTable(db, 44, 10, 3, cell);
                                break;
                            case (int)TipProds.Tisnenie:
                                Read2DTable(db, 401, 7, 3, cell);
                                ReadParam(db, 3, 3, cell);
                                ReadParam(db, 4, 3, cell);
                                break;
                            case (int)TipProds.UFkachestvo:
                                Read2DTable(db, 169, 3, 3, cell, true);
                                Read2DTable(db, 170, 7, 3, cell, true);
                                Read2DTable(db, 171, 11, 3, cell, true);
                                Read2DTable(db, 172, 15, 3, cell);
                                break;
                            case (int)TipProds.UFstandart:
                                Read2DTable(db, 298, 3, 3, cell);
                                Read2DTable(db, 299, 8, 3, cell);
                                Read2DTable(db, 300, 13, 3, cell);
                                break;
                            case (int)TipProds.Shelkografiya:
                                Read2DTable(db, 35, 3, 3, cell);
                                Read2DTable(db, 36, 10, 3, cell);
                                Read2DTable(db, 158, 17, 3, cell);
                                break;
                            case (int)TipProds.Banner:
                                Read2DTable(db, 2, 6, 5, cell);
                                Read2DTable(db, 3, 19, 5, cell);

                                foreach (var row in new int[] { 35, 36, 37, 38, 39, 40, 41, 42, 43 })
                                    ReadParam(db, row, 5, cell);
                                break;
                            case (int)TipProds.Vimpel:
                                Read2DTable(db, 431, 2, 3, cell);
                                break;
                            case (int)TipProds.Znachok:
                                Read2DTable(db, 414, 2, 3, cell);
                                break;
                            case (int)TipProds.KontrBraslet:
                                foreach (var row in new int[] { 3, 12, 22 })
                                    ReadTirazPriceTable(db, row, 1, cell);

                                foreach (var row in new int[] { 6, 7, 8, 15, 16, 17, 25, 26 })
                                    ReadParam(db, row, 9, cell);
                                break;
                            case (int)TipProds.Lenta:
                                foreach (var row in new int[] { 3, 14, 26, 38, 50, 62 })
                                    ReadTirazPriceTable(db, row, 1, cell);

                                foreach (var row in new int[] {
                    6
                    ,17,18,19,20,21
                    ,29,30,31,32
                    ,41,42,43,44
                    ,53,54,55,56
                    ,65,66
                })
                                    ReadParam(db, row, 9, cell);

                                break;
                            case (int)TipProds.Platok:
                                Read2DTable(db, 434, 2, 3, cell);
                                break;
                            case (int)TipProds.ReklNakidka:
                                Read2DTable(db, 435, 2, 3, cell, true);

                                break;
                            case (int)TipProds.Svetootrazatel:
                                ReadTirazPriceTable(db, 3, 1, cell);
                                ReadTirazPriceTable(db, 12, 1, cell);
                                ReadTirazPriceTable(db, 23, 1, cell);
                                ReadTirazPriceTable(db, 33, 1, cell);

                                foreach (var row in new int[] {
                    6,7,8
                    ,15,16,17,18
                    ,26,27,28,29
                    ,36,37,38,39
                })
                                    ReadParam(db, row, 9, cell);
                                break;
                            case (int)TipProds.SiliconBraslet:
                                foreach (var row in new int[] { 3, 16, 28, 39, 51, 60, 71, 82, 92, 102, 109, 117, 127, 135 })
                                    ReadTirazPriceTable(db, row, 1, cell);

                                foreach (var row in new int[] {
                    6,7,8,9,10,11,12
                    ,19,20,21,22,23
                    ,31,32,33,34
                    ,42,43,44,45
                    ,54,55
                    ,63,64,65,66,67
                    ,74,75,76,77
                    ,85,86,87,88
                    ,95,96
                    ,105
                    ,112,113,114
                    ,120,121,122,123,124
                    ,130,131,132
                    ,138,139,140
                })
                                    ReadParam(db, row, 9, cell);
                                break;
                            case (int)TipProds.SlapChasi:
                                ReadTirazPriceTable(db, 3, 1, cell);
                                ReadTirazPriceTable(db, 12, 1, cell);
                                ReadTirazPriceTable(db, 22, 1, cell);

                                foreach (var row in new int[] {
                    6,7,8,9
                    ,15,16,17,18,19
                    ,25,26,27,28,29,30
                })
                                    ReadParam(db, row, 9, cell);
                                break;
                            case (int)TipProds.Skatert:
                                Read2DTable(db, 432, 2, 3, cell);
                                break;
                            case (int)TipProds.SlapBraslet:
                                ReadTirazPriceTable(db, 3, 1, cell);
                                ReadTirazPriceTable(db, 13, 1, cell);
                                ReadTirazPriceTable(db, 23, 1, cell);

                                foreach (var row in new int[] {
                    6,7,8,9
                    ,16,17,18,19,20
                })
                                    ReadParam(db, row, 9, cell);
                                break;
                            case (int)TipProds.SportNomer:
                                Read2DTable(db, 436, 2, 3, cell);
                                break;
                            case (int)TipProds.Flag:
                                Read2DTable(db, 428, 2, 3, cell);
                                break;
                            case (int)TipProds.FlagNSO:
                                Read2DTable(db, 430, 2, 3, cell, true);
                                break;
                            case (int)TipProds.FlagPobedi:
                                Read2DTable(db, 429, 2, 3, cell);
                                break;
                            case (int)TipProds.Sharf:
                                Read2DTable(db, 433, 2, 3, cell);
                                break;
                            case (int)TipProds.BumajniiPaket:
                                Read2DTable(db, 419, 2, 3, cell);
                                break;
                            case (int)TipProds.PaketPvd:
                                ReadSingleRowTable(db, 412, 3, 3, cell);

                                Read2DTable(db, 413, 12, 3, cell);
                                break;
                            default:
                                throw new Exception("no product sheet");
                                break;
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var msg = $"Ошибка на листе {application.ActiveSheet} в ячейке {application.ActiveCell.Address}: {ex.Message}";
                    return msg;
                }/**/
                //MainWindow.p.Dispatcher.Invoke((Action)(() => { progressBar1.Value = progressBar1.Maximum; }));
                //MessageBox.Show("Импорт завершен!");
            }
            return "";
        }


        private static void ReadParam(kvotaEntities db, int hi, int hj, IRange cell)
        {
            cell[hi, hj].Activate();
            int id = GetInt(cell[hi, hj]);

            var exist = (from pp in db.Price
                         where pp.catId == id && pp.firma == (int)Postavs.Плановая_СС
                         select pp).ToList();
            if (exist.Count != 1)
                throw new Exception("param not 1");

            cell[hi, hj + 1].Activate();
            var s = cell[hi, hj + 1];
            double cena = 0;
            if (!CellEmpty(s))
                cena = GetDouble(s);

            if (IsCompare) if (cena != exist[0].cena) throw new Exception("param");
            exist[0].cena = cena;
        }

        private static void ReadTirazPriceTable(kvotaEntities db, int hi, int hj, IRange cell, bool noParent = false)
        {
            int i = 2, j = 3, maxJ, empLines = 1, tiraz;

            i = hi + 1; maxJ = hj + 1;
            var exist = new List<Price>();
            while (!CellEmpty(cell[i, maxJ]))
            {
                cell[i, maxJ].Activate();
                int parentId = GetInt(cell[i, maxJ]);
                exist.AddRange(from pp in db.Price
                               where pp.catId == parentId && pp.firma == (int)Postavs.Плановая_СС
                               orderby pp.catId, pp.tiraz
                               select pp);
                maxJ++;
            }

            var newlist = new List<Price>();
            double cena = 0;
            i = hi + 1;
            while (!CellEmpty(cell[++i, hj]))
            {
                cell[i, hj].Activate();
                tiraz = GetInt(cell[i, hj]);
                j = 1;
                while (hj + j < maxJ)//(cell[i, hj + j].Text) != "")
                {
                    cell[i, hj + j].Activate();
                    var s = cell[i, hj + j];
                    if (!CellEmpty(s))
                    {
                        cena = GetDouble(s);

                        cell[hi, hj + j].Activate();
                        var id = GetInt(cell[hi + 1, hj + j]);
                        var p = new Price { catId = id, firma = (int)Postavs.Плановая_СС, cena = cena, tiraz = tiraz };
                        newlist.Add(p);
                    }
                    j++;
                }
            }
            newlist = newlist.OrderBy(pp => pp.catId).ThenBy(pp => pp.tiraz).ToList();

            if (IsCompare) Compare(newlist, exist);
            db.Price.RemoveRange(exist);
            db.Price.AddRange(newlist);
        }

        private static void ReadSingleRowTable(kvotaEntities db, int parentId, int hi, int hj, IRange cell, bool noParent = false)
        {
            var exist = (from pp in db.Price
                         join cc in db.Category on pp.catId equals cc.id
                         where cc.parentId == parentId && pp.firma == (int)Postavs.Плановая_СС
                         orderby pp.catId, pp.tiraz
                         select pp).ToList();
            var newlist = new List<Price>();
            int i = 2, j = 3, empLines = 1, id;
            double cena = 0;
            i = hi;
            while (!CellEmpty(cell[++i, hj]))
            {
                cell[i, hj].Activate();
                id = GetInt(cell[i, hj]);
                cell[i, hj + 1].Activate();
                var s = cell[i, hj + 1];
                if (!CellEmpty(s))
                {
                    cena = GetDouble(s);
                    var p = new Price { catId = id, firma = (int)Postavs.Плановая_СС, cena = cena };
                    newlist.Add(p);
                }
            }
            newlist = newlist.OrderBy(pp => pp.catId).ThenBy(pp => pp.tiraz).ToList();

            if (IsCompare) Compare(newlist, exist);
            db.Price.RemoveRange(exist);
            db.Price.AddRange(newlist);
        }

        private static void Read2DTable(kvotaEntities db, int parentId, int hi, int hj, IRange cell, bool noParent = false)
        {
            List<Price> exist;
            if (noParent)
                exist = (from pp in db.Price
                         where pp.catId == parentId && pp.firma == (int)Postavs.Плановая_СС
                         orderby pp.catId, pp.tiraz
                         select pp).ToList();
            else
                exist = (from pp in db.Price
                         join cc in db.Category on pp.catId equals cc.id
                         where cc.parentId == parentId && pp.firma == (int)Postavs.Плановая_СС
                         orderby pp.catId, pp.tiraz
                         select pp).ToList();
            var newlist = new List<Price>();
            int i = 2, j = 3, id;
            double cena = 0;
            i = hi;
            while (!CellEmpty(cell[++i, hj]))
            {
                cell[i, hj].Activate();
                id = GetInt(cell[i, hj]);
                j = 1;
                while (!CellEmpty(cell[i, hj + j]))
                {
                    cell[i, hj + j].Activate();
                    var s = cell[i, hj + j];
                    if (!CellEmpty(s))
                    {
                        cena = GetDouble(s);

                        cell[hi, hj + j].Activate();
                        var tiraz = GetInt(cell[hi, hj + j]);
                        var p = new Price { catId = id, firma = (int)Postavs.Плановая_СС, cena = cena, tiraz = tiraz };
                        newlist.Add(p);
                    }
                    j++;
                }
            }

            if (IsCompare) Compare(newlist, exist);
            db.Price.RemoveRange(exist);
            db.Price.AddRange(newlist);
        }

        private static void Compare(List<Price> newlist, List<Price> exist)
        {
            newlist = newlist.OrderBy(pp => pp.catId).ThenBy(pp => pp.tiraz).ToList();
            if (newlist.Count == exist.Count && exist.Count > 0)
            {
                for (int k = 0; k < exist.Count; k++)
                {
                    var ex = exist[k]; var nw = newlist[k];
                    if (ex.catId != ex.catId || ex.tiraz != ex.tiraz || ex.cena != ex.cena)
                        throw new Exception("fields");
                }
            }
            else throw new Exception("Count");
        }


        private static bool CellEmpty(IRange cell)
        {
            if (cell.Value2 == null)
                return true;

            var text = cell.HasFormula ? cell.FormulaNumberValue.ToString() : cell.Value2.ToString();
            return text == "";
        }

        private static int GetInt(IRange cell)
        {
            var text = cell.HasFormula ? cell.FormulaNumberValue.ToString() : cell.Value2.ToString();
            double id = 0;
            if (double.TryParse(text, out id) && (int)id == id)
                return (int)id;
            throw new Exception("wrong int");
        }

        private static double GetDouble(IRange cell)
        {
            string s = cell.HasFormula ? cell.FormulaNumberValue.ToString() : cell.Value2.ToString();
            s = s.Replace(".", ",").Replace(" ", "").Trim();
            double cena = 0;

            if (double.TryParse(s, out cena))
            {
                return Math.Round(cena, 2);
            }
            throw new Exception("wrong float");
        }


    }
}