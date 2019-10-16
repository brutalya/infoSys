using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    class Data
    {
        public static string[] tableNames = new string[] { "Products", "Supp", "Manufacturer", "Recipe" , "City" , "Prices" , "Categories" , "Storage" , "Supply" , "Selling","SupplyDetails" };
        /*tablenames[0] = "Products";
            tablenames[1] = "Supp";
            tablenames[2] = "Manufacturer";
            tablenames[3] = "Recipe";
            tablenames[4] = "City";
            tablenames[5] = "Prices";
            tablenames[6] = "Categories";
            tablenames[7] = "Storage";
            tablenames[8] = "Supply";
            tablenames[9] = "Selling";*/
        public static Dictionary<string, string> [] comboboxNames = new Dictionary<string, string>[11];
        public static Dictionary<string, string> [] comboboxHeads = new Dictionary<string, string>[11];
        public static string[][] columnsHeads = new string[11][];
        public static void DataVoid()
        {
            //заполнение combobox
            comboboxNames[0] = new Dictionary<string, string>();
            comboboxNames[0].Add("manuId", "Manufacturer");
            comboboxNames[0].Add("categoryId", "Categories");
            comboboxNames[1] = new Dictionary<string, string>();
            comboboxNames[1].Add("cityId", "City");
            comboboxNames[2] = new Dictionary<string, string>();
            comboboxNames[2].Add("cityId", "City");
            comboboxNames[3] = new Dictionary<string, string>();
            comboboxNames[4] = new Dictionary<string, string>();
            comboboxNames[5] = new Dictionary<string, string>();
            comboboxNames[5].Add("prodId", "Products");
            comboboxNames[6] = new Dictionary<string, string>();
            comboboxNames[7] = new Dictionary<string, string>();
            comboboxNames[8] = new Dictionary<string, string>();
            comboboxNames[8].Add("suppId", "Supp");
            comboboxNames[8].Add("storId", "Storage");
            comboboxNames[9] = new Dictionary<string, string>();
            comboboxNames[9].Add("storId", "Storage");
            comboboxNames[10] = new Dictionary<string, string>();
            comboboxNames[10].Add("supplyId", "Supply");
            comboboxNames[10].Add("prodId", "Products");
            //заполнение Heads
            comboboxHeads[0] = new Dictionary<string, string>();
            comboboxHeads[0].Add("manuId", "Производитель");
            comboboxHeads[0].Add("categoryId", "Категория");
            comboboxHeads[1] = new Dictionary<string, string>();
            comboboxHeads[1].Add("cityId", "Город");
            comboboxHeads[2] = new Dictionary<string, string>();
            comboboxHeads[2].Add("cityId", "Город");
            comboboxHeads[3] = new Dictionary<string, string>();
            comboboxHeads[4] = new Dictionary<string, string>();
            comboboxHeads[5] = new Dictionary<string, string>();
            comboboxHeads[5].Add("prodId", "Товар");
            comboboxHeads[6] = new Dictionary<string, string>();
            comboboxHeads[7] = new Dictionary<string, string>();
            comboboxHeads[8] = new Dictionary<string, string>();
            comboboxHeads[8].Add("suppId", "Поставщик");
            comboboxHeads[8].Add("storId", "Магазин");
            comboboxHeads[9] = new Dictionary<string, string>();
            comboboxHeads[9].Add("storId", "Магазин");
            comboboxHeads[10] = new Dictionary<string, string>();
            comboboxHeads[10].Add("supplyId", "Supply");
            comboboxHeads[10].Add("prodId", "Товар");
            //заполнение columnsHeads
            columnsHeads[0] = new string[] { "Наименование","Производитель","Категория" };
            columnsHeads[1] = new string[] { "Наименование","Адрес","ИНН","Р./С.","Город" };
            columnsHeads[2] = new string[] { "Наименование","ИНН","Адрес","Город" };
            columnsHeads[3] = new string[] { "Необходимость в рецепте" };
            columnsHeads[4] = new string[] { "Наименование"};
            columnsHeads[5] = new string[] { "Оптовая цена", "Розничная цена","Товар" };
            columnsHeads[6] = new string[] { "Наименование" };
            columnsHeads[7] = new string[] { "Наименование", "Адрес" };
            columnsHeads[8] = new string[] { "Дата и время","Поставщик", "Магазин" };
            columnsHeads[9] = new string[] { "Дата и время","Магазин" };
            columnsHeads[10] = new string[] { "Поставщик", "Товар","Количество" };
        }
    }
}
