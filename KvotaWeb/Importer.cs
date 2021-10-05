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
        internal static object Import(string filename = @"C:\Downloads\РАБОТА @\Калькулятор КВОТА 2.0\Rossuvenir\калькулятор 2020.04.01 - единый для импорта\калькулятор 2021.09.12 - шаблон.xlsx")
        {
            FileStream inputStream = new FileStream(filename, FileMode.Open);
            return Import(inputStream);
        }

        public static string Import(Stream inputStream)
        {
            kvotaEntities db = new kvotaEntities();

            db.Price.RemoveRange(db.Price.Where(x => x.firma.HasValue));

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
                        var tipProd = GetInt(cell[1, 1]);

                        if (string.IsNullOrEmpty(cell["G1"].Value2.ToString())) throw new Exception("Не задан поставщик");

                        var firmaName=cell["G1"].Value2.ToString();

                        //db.Firma.RemoveRange(db.Firma);
                        var firma = db.Firma.FirstOrDefault(pp => pp.name == firmaName);
                        if (firma == null)
                            using (var db2 = new kvotaEntities())
                            {
                                db2.Firma.Add(firma = new Firma() { name = firmaName });
                                db2.SaveChanges();
                            }
                        var firmaId = firma.id;

                        //if (!new[] { (int)TipProds.UFkachestvo, (int)TipProds.UFstandart }.Contains(tipProd)) continue;//TODO del
                        switch (tipProd)
                        {
                            case (int)TipProds.DTG:
                                Read2DTable(db, 0, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.Gravirovka:
                                foreach (var row in new int[] {6,13,20,29,41,51,61,65})
                                Read2DTable(db, 0, row, 3, cell, firmaId);
                                ReadSingleColumnTable(db, 0, 35, 3, cell, firmaId);

                                foreach (var row in new int[] {
                                    9,10,
                                    16,17,
                                    23,24,25,26,
                                    32,
                                    44,45,46,47,
                                    58
                                })
                                    ReadParam(db, row, 3, cell,firmaId);
                                break;

                            case (int)TipProds.Decol:
                                Read2DTable(db, 307, 3, 3, cell,firmaId);
                                ReadParam(db, 9, 3, cell,firmaId);

                                Read2DTable(db, 308, 12, 3, cell,firmaId);

                                Read2DTable(db, 0, 21, 3, cell, firmaId);
                                break;
                            case (int)TipProds.Tampopechat:
                                foreach (var row in new int[] { 3,10,17,24,31 })
                                    Read2DTable(db, 0, row, 3, cell, firmaId);
                                break;
                            case (int)TipProds.Tisnenie:
                                foreach (var row in new int[] { 4,10,18,23 })
                                    Read2DTable(db, 0, row, 3, cell, firmaId);

                                ReadParam(db, 16, 3, cell,firmaId);
                                ReadParam(db, 21, 3, cell,firmaId);
                                break;
                            case (int)TipProds.UFkachestvo:
                                foreach (var row in new int[] { 3,10,16,23,30,41,58})
                                    Read2DTable(db, 0, row, 3, cell, firmaId);

                                /*Read2DTable(db, 169, 3, 3, cell,firmaId, true);
                                Read2DTable(db, 170, 7, 3, cell,firmaId, true);
                                Read2DTable(db, 171, 11, 3, cell,firmaId, true);
                                Read2DTable(db, 172, 15, 3, cell,firmaId);

                                Read2DTable(db, 640, 26, 3, cell,firmaId, true);
                                Read2DTable(db, 641, 31, 3, cell,firmaId, true);*/
                                break;
                            case (int)TipProds.Shelkografiya:
                                Read2DTable(db, 0, 3, 3, cell, firmaId);
                                ReadSingleColumnTable(db, 0, 12, 3, cell, firmaId);
                                break;
                                
                            case (int)TipProds.PaketPvd:
                                Read2DTable(db, 0, 4, 3, cell, firmaId);
                                ReadParam(db, 10, 3, cell, firmaId);
                                ReadSingleColumnTable(db, 0, 13, 3, cell, firmaId);

                                break;
                            /*case (int)TipProds.Banner:
                                Read2DTable(db, 2, 6, 5, cell,firmaId);
                                Read2DTable(db, 3, 19, 5, cell,firmaId);

                                foreach (var row in new int[] { 35, 36, 37, 38, 39, 40, 41, 42, 43 })
                                    ReadParam(db, row, 5, cell,firmaId);
                                break;
                            case (int)TipProds.Vimpel:
                                Read2DTable(db, 431, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.Znachok:
                                Read2DTable(db, 414, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.KontrBraslet:
                                foreach (var row in new int[] { 3, 12, 22 })
                                    ReadTirazPriceTable(db, row, 1, cell,firmaId);

                                foreach (var row in new int[] { 6, 7, 8, 15, 16, 17, 25, 26 })
                                    ReadParam(db, row, 9, cell,firmaId);
                                break;
                            case (int)TipProds.Lenta:
                                foreach (var row in new int[] { 3, 14, 26, 38, 50, 62 })
                                    ReadTirazPriceTable(db, row, 1, cell,firmaId);

                                foreach (var row in new int[] {
                    6
                    ,17,18,19,20,21
                    ,29,30,31,32
                    ,41,42,43,44
                    ,53,54,55,56
                    ,65,66
                })
                                    ReadParam(db, row, 9, cell,firmaId);

                                break;
                            case (int)TipProds.Platok:
                                Read2DTable(db, 434, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.ReklNakidka:
                                Read2DTable(db, 435, 2, 3, cell,firmaId, true);

                                break;
                            case (int)TipProds.Svetootrazatel:
                                ReadTirazPriceTable(db, 3, 1, cell,firmaId);
                                ReadTirazPriceTable(db, 12, 1, cell,firmaId);
                                ReadTirazPriceTable(db, 23, 1, cell,firmaId);
                                ReadTirazPriceTable(db, 33, 1, cell,firmaId);

                                foreach (var row in new int[] {
                    6,7,8
                    ,15,16,17,18
                    ,26,27,28,29
                    ,36,37,38,39
                })
                                    ReadParam(db, row, 9, cell,firmaId);
                                break;
                            case (int)TipProds.SiliconBraslet:
                                foreach (var row in new int[] { 3, 16, 28, 39, 51, 60, 71, 82, 92, 102, 109, 117, 127, 135 })
                                    ReadTirazPriceTable(db, row, 1, cell,firmaId);

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
                                    ReadParam(db, row, 9, cell,firmaId);
                                break;
                            case (int)TipProds.SlapChasi:
                                ReadTirazPriceTable(db, 3, 1, cell,firmaId);
                                ReadTirazPriceTable(db, 12, 1, cell,firmaId);
                                ReadTirazPriceTable(db, 22, 1, cell,firmaId);

                                foreach (var row in new int[] {
                    6,7,8,9
                    ,15,16,17,18,19
                    ,25,26,27,28,29,30
                })
                                    ReadParam(db, row, 9, cell,firmaId);
                                break;
                            case (int)TipProds.Skatert:
                                Read2DTable(db, 432, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.SlapBraslet:
                                ReadTirazPriceTable(db, 3, 1, cell,firmaId);
                                ReadTirazPriceTable(db, 13, 1, cell,firmaId);
                                ReadTirazPriceTable(db, 23, 1, cell,firmaId);

                                foreach (var row in new int[] {
                    6,7,8,9
                    ,16,17,18,19,20
                })
                                    ReadParam(db, row, 9, cell,firmaId);
                                break;
                            case (int)TipProds.SportNomer:
                                Read2DTable(db, 436, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.Flag:
                                Read2DTable(db, 428, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.FlagNSO:
                                Read2DTable(db, 430, 2, 3, cell,firmaId, true);
                                break;
                            case (int)TipProds.FlagPobedi:
                                Read2DTable(db, 429, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.Sharf:
                                Read2DTable(db, 433, 2, 3, cell,firmaId);
                                break;
                            case (int)TipProds.BumajniiPaket:
                                Read2DTable(db, 419, 2, 3, cell,firmaId);
                                break;*/

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


        private static void ReadParam(kvotaEntities db, int hi, int hj, IRange cell,int firmaId)
        {
            cell[hi, hj].Activate();
            int id = GetInt(cell[hi, hj]);

            /*var exist = (from pp in db.Price
                         where pp.catId == id && pp.firma == firmaId
                         select pp).ToList();
            if (exist.Count != 1)
                throw new Exception("param not 1");*/

            cell[hi, hj + 1].Activate();
            var s = cell[hi, hj + 1];
            double cena = 0;
            if (!CellEmpty(s))
            {
                cena = GetDouble(s);
                db.Price.Add(new Price() { catId = id, firma = firmaId, cena = cena });
            }

            //if (IsCompare) if (cena != exist[0].cena) throw new Exception("param");
            //exist[0].cena = cena;
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
                               where pp.catId == parentId && pp.firma0 == (int)Postavs.Плановая_СС
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
                        var p = new Price { catId = id, firma0 = (int)Postavs.Плановая_СС, cena = cena, tiraz = tiraz };
                        newlist.Add(p);
                    }
                    j++;
                }
            }
            newlist = newlist.OrderBy(pp => pp.catId).ThenBy(pp => pp.tiraz).ToList();

            if(!newlist.Any()) throw new Exception("no values");
            if (IsCompare) Compare(newlist, exist);
            db.Price.RemoveRange(exist);
            db.Price.AddRange(newlist);
        }

        private static void ReadSingleColumnTable(kvotaEntities db, int parentId, int hi, int hj, IRange cell,int firmaId)
        {
            /*var exist = (from pp in db.Price
                         join cc in db.Category on pp.catId equals cc.id
                         where cc.parentId == parentId && pp.firma == firmaId
                         orderby pp.catId, pp.tiraz
                         select pp).ToList();*/
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
                    var p = new Price { catId = id, firma = firmaId, cena = cena };
                    newlist.Add(p);
                }
            }
            newlist = newlist.OrderBy(pp => pp.catId).ThenBy(pp => pp.tiraz).ToList();

            //if (!newlist.Any()) throw new Exception("no values");
            //if (IsCompare) Compare(newlist, exist);
            //db.Price.RemoveRange(exist);
            db.Price.AddRange(newlist);
        }

        private static void Read2DTable(kvotaEntities db, int parentId, int hi, int hj, IRange cell, int firmaId, bool noParent = false)
        {
            /*List<Price> exist;
            if (noParent)
                exist = (from pp in db.Price
                         where pp.catId == parentId && pp.firma == firmaId
                         orderby pp.catId, pp.tiraz
                         select pp).ToList();
            else
                exist = (from pp in db.Price
                         join cc in db.Category on pp.catId equals cc.id
                         where cc.parentId == parentId && pp.firma == firmaId
                         orderby pp.catId, pp.tiraz
                         select pp).ToList();*/
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
                        var tiraz = GetTiraz(cell[hi, hj + j]);
                        var p = new Price { catId = id, firma = firmaId, cena = cena, tiraz = tiraz.Tiraz,isAllTiraz=tiraz.IsAllTiraz };
                        newlist.Add(p);
                    }
                    j++;
                }
            }

            //if (!newlist.Any()) throw new Exception("no values");
            //if (IsCompare) Compare(newlist, exist);
            //db.Price.RemoveRange(exist);
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

        private class TirazValue
        {
            public int Tiraz { get; set; }
            public bool IsAllTiraz { get; set; }
        }

        private static TirazValue GetTiraz(IRange cell)
        {
            var text = cell.HasFormula ? cell.FormulaNumberValue.ToString() : cell.Value2.ToString();

            var isAllTiraz = false;
            if (text.EndsWith("*"))
            {
                isAllTiraz = true;
                text = text.Substring(0, text.Length - 1);
            }
            double id;
            if (double.TryParse(text, out  id) && (int)id == id)
                return new TirazValue { Tiraz = (int)id, IsAllTiraz = isAllTiraz };

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