/**
 * Zaznam hash tabulky s nahledy obrazku
 */
public class Entry
{
    /** Nazev souboru s obrazkem */
    public String fileName;
    /** Index v databazi nahledu obrazku */
    public int indexInDatabase;
    /** Dalsi zaznam (pro potreby hash tabulky) */
    public Entry next;

    /**
	 * Vyvtori novy zaznam se zadanym nazvem souboru a indexem do databaze
	 */
    public Entry(String fn, int index)
    {
        fileName = fn;
        indexInDatabase = index;
    }
}

/**
 * Hash tabulka pro uchovani zaznamu s nahledem obrazku
 */
public class HashTable
{
    /** Zaznamy tabulky */
    public Entry[] data;

    /**
	 * Vytvori novou hash tabulku se zadanou kapacitou
	 */
    public HashTable(int capacity)
    {
        data = new Entry[capacity];
    }
    
    /**
	 * Prida zaznam do has tabulky
	 */
    public void Add(String key, int value)
    {
        Entry newEntry = new Entry(key, value);
        int index = GetHashCode(key);
        newEntry.next = data[index];
        data[index] = newEntry;
    }

    /// <summary>
    /// Získá záznam z hash tabulky. 
    /// </summary>
    /// <param name="key">Hledaný záznam</param>
    /// <returns>Index v databázy záznamu pokud je nalezen, jinak -1</returns>
    public int Get(String key)
    {
        int hash = GetHashCode(key);
        Entry current = data[hash];
        while(current != null)
        {
            if (current.fileName == key)
            {
                return (current.indexInDatabase);
            }
            current = current.next;
        }
        return -1;
    }

    /**
	 * Vypocte a vrati hash kod pro zadany klic
	 */
    
    public virtual int GetHashCode(String s)
    {
        int result = s[0];
        for (int i = 1; i < s.Length; i++)
        {
            result = (result * 256 + s[i]) % data.Length;
        }

        return result;
    }
}

/// <summary>
/// Třída reprezentuje hash tabulku s přetíženou metodou pro získání hodnoty hash pro zadaný klíč
/// </summary>
public class HashTableSecond : HashTable
{
    /// <summary>
    /// Kontruktor třídy <b>HashTableSecond</b>.
    /// </summary>
    /// <param name="capacity">Velikost hash tabulky</param>
    public HashTableSecond(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// Vypočte a vrátí hodnotu hash kódu pro zadaný klíč.
    /// </summary>
    /// <param name="s">Klíč</param>
    /// <returns>Vrátí vždy 0</returns>
    public override int GetHashCode(String s)
    {
        return 0;
    }
}

/**
 * Hlavni trida programu
 */
public class Thumbnails
{
    static Random r = new Random();

    public static void Main(String[] args)
    {
        //HashTable table = new HashTable(100);
        //table.Add("c:\\img3451.jpg", 0);
        //table.Add("c:\\img7657.jpg", 1);
        //table.Add("c:\\img0987.jpg", 2);

        //Console.WriteLine(table.Get("c:\\img7687.jpg"));
        /*
        Console.Write("Zadejte název souboru: ");
        string inp = Console.ReadLine();
        Console.WriteLine(table.Get(inp.Trim()));
        */

        string[] fileNames = File.ReadAllLines("ImageNames.txt");
        int[] sizes = {1_000, 1_009, 30_030, 100_000, 100_003};

        Console.WriteLine("Počet zpracovaných dotazů za 2 sekundy, hloupá rozptylová funkce:");
        for(int i = 0; i < sizes.Length; i++)
        {
            TestHashType(fileNames, new HashTableSecond(sizes[i]));
        }

        Console.WriteLine("Počet zpracovaných dotazů za 2 sekundy, chytrá rozptylová funkce:");
        for (int i = 0; i < sizes.Length; i++)
        {
            TestHashType(fileNames, new HashTable(sizes[i]));
        }

        /*   Ukol 2
        for(int i = 0; i < 1000; i++)
        {
            table.Add(RandomImageName(), i);
        }

        Console.WriteLine(table.data.Length);

        for(int i = 0; i < table.data.Length; i++)
        {
            int count = 0;
            Entry s = table.data[i];
            while(s != null)
            {
                count++;
                s = s.next;
            }
            Console.WriteLine($"{table.data[i]} - {count}");
        }
        */
    }

    /// <summary>
    /// Metoda otestuje různé typy hashování a spočítá, jak dlouho trvá zpracování různých počtů dotazů.
    /// </summary>
    /// <param name="fileNames">Názvy souborů</param>
    /// <param name="tab">Testovaná hash tabulka</param>
    public static void TestHashType(string[] fileNames, HashTable tab)
    {
        DateTime start = DateTime.Now;
        int count = 0;

        while ((DateTime.Now - start).TotalSeconds <= 2)
        {
            int a = r.Next(fileNames.Length);

            string fname = fileNames[a];

            if (tab.Get(fname) == -1)
            {
                tab.Add(fname, 0);
            }
            count++;
        }
        
        Console.WriteLine($"Pro C = {tab.data.Length} bylo zpracováno {count} dotazů");
    }

    /**
	 * Vygeneruje a vrati nahodny nazev obrazku
	 */
    private static String RandomImageName()
    {
        Random r = new Random();
        int year = 2005 + r.Next(13);
        int month = 1 + r.Next(12);
        int day = 1 + r.Next(28);
        int img = 1 + r.Next(9999);
        return String.Format("c:\\fotky\\{0}-{1}-{2}\\IMG_{3}.CR2", year, month, day, img);
    }
}