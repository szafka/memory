namespace bell
{
    class Memory
    {
        String[][] matrix = null;
        Boolean[][] guessedMatrix = null;

        int chances;

        int sizeColumn;

        static void Main(string[] args)
        {
            Memory memory = new Memory();
            memory.start();
        }

        void start()
        {
            Console.WriteLine("Choose level: Easy or Hard (E/H)");

            //zczytywanie wartości z konsoli
            String level = Console.ReadLine();
            level = level.ToUpper();

            string[] lines = readWordsArray();
            //wyswietlanie wartosci => ToUpper sprawdz co sie stanie jak uruchomisz program i wprowadzisz wartość "e" i wartosc "E" 
            if (level.Equals("E"))
            {
                Console.WriteLine("Selected level: Easy");
                chances = 10;
                sizeColumn = 4;
                InitLevel(level, lines);
            }
            else if (level.Equals("H"))
            {
                Console.WriteLine("Selected level: Hard");
                chances = 15;
                sizeColumn = 8;
                InitLevel(level, lines);
            }
            else
            {
                Console.WriteLine("Selected wrong level. Bye Bye");
                //Console.ReadKey();
            }

            while (!IsGameOver())
            {
                PrintMatrix();
                PlayRound();
            }

            Console.Write("Game over. You ");
            if (IsAllGuessed())
            {
                Console.WriteLine("win.");
            }
            else
            {
                Console.WriteLine("lose.");
            }
        }
        //String level pobierze nam z main zmienną level

        void InitLevel(String level, string[] lines)
        {

            matrix = new String[2][];
            matrix[0] = new String[sizeColumn];
            matrix[1] = new String[sizeColumn];

            guessedMatrix = new Boolean[2][];
            guessedMatrix[0] = new Boolean[sizeColumn];
            guessedMatrix[1] = new Boolean[sizeColumn];

            List<String> linesList = lines.ToList();
            //mieszanie listy
            List<String> shuffledLinesListAll = linesList.OrderBy(i => Guid.NewGuid()).ToList();
            //linesList.AddRange(shuffledLinesListAll);
            List<String> shortList = shuffledLinesListAll.GetRange(0, sizeColumn);
            shortList.AddRange(shortList);
            shortList = shortList.OrderBy(i => Guid.NewGuid()).ToList();

            //Console.WriteLine(shortList.Count);  długość listy

            int index = 0;
            foreach (string line in shortList)
            {
                //Console.WriteLine(line);
                int y, x;
                y = index / sizeColumn; //wiersz
                x = index % sizeColumn; //kolumna
                // sprawdzenie poprawności zapisania indeksów: Console.WriteLine("y " + y + "x "+ x);

                matrix[y][x] = line;
                guessedMatrix[y][x] = false;
                index++;

            }


        }

        void PrintMatrix()
        {
            Console.WriteLine("Guess chances: " + chances);
            Console.WriteLine();
            Console.Write("\t");
            for (int i = 0; i < sizeColumn; i++)
            {
                Console.Write(i + 1);
                Console.Write("\t");
            }
            Console.WriteLine();
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write((char)(65 + i));
                Console.Write("\t");
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (guessedMatrix[i][j])
                    {
                        Console.Write(matrix[i][j]); //wyświetlanie po indeksach
                    }
                    else
                    {
                        Console.Write('X'); //wyświetlanie po indeksach
                    }
                    Console.Write("\t");

                    //Console.WriteLine("arr[" + i + "][" + j + "] = " + arr[i][j]);
                }
                Console.WriteLine();
            }
        }

        string ReadCoordinates()
        {
            Console.WriteLine("Choose coordinates");
            String coord = Console.ReadLine();
            return coord.ToUpper();
        }

        string[] readWordsArray()
        {
            return System.IO.File.ReadAllLines("words.txt");
        }




        //static void writeWords(string[] words)
        //{
        //   foreach (string word in words)
        //   {
        //       Console.WriteLine(word);
        //   }

        //}
        void PlayRound()
        {
            int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
            Boolean coordValid = false;

            do
            {
                String coord = ReadCoordinates();

                if (coord.Length == 2)
                {
                    x1 = coord[0] - 65;
                    y1 = coord[1] - 48 - 1;
                    coordValid = (x1 >= 0 && x1 < matrix.Length) && (y1 >= 0 && y1 < sizeColumn);
                    if (!coordValid)
                    {
                        Console.WriteLine("Coordinates not found. Try again.");
                    }
                }
                else
                {
                    coordValid = false;
                    Console.WriteLine("Coordinates not found. Try again.");
                }

            } while (!coordValid);

            // odsłoń wyraz
            Boolean oldValue1 = guessedMatrix[x1][y1];
            guessedMatrix[x1][y1] = true;
            PrintMatrix();

            do
            {
                String coord = ReadCoordinates();

                if (coord.Length == 2)
                {
                    x2 = coord[0] - 65;
                    y2 = coord[1] - 48 - 1;
                    coordValid = (x2 >= 0 && x2 < matrix.Length) && (y2 >= 0 && y2 < sizeColumn);
                    if (!coordValid)
                    {
                        Console.WriteLine("Coordinates not found. Try again.");
                    }
                }
                else
                {
                    coordValid = false;
                    Console.WriteLine("Coordinates not found. Try again.");
                }

            } while (!coordValid);

            // odsłoń wyraz 
            Boolean oldValue2 = guessedMatrix[x2][y2];
            guessedMatrix[x2][y2] = true;
            PrintMatrix();
            Console.WriteLine("Press key to continue");
            Console.ReadKey();

            // recalculate chances
            if (!matrix[x1][y1].Equals(matrix[x2][y2]))
            {
                chances--;
                guessedMatrix[x1][y1] = oldValue1;
                guessedMatrix[x2][y2] = oldValue2;
            }
        }

        Boolean IsGameOver()
        {
            if (chances <= 0)
            {
                return true;
            }
            return IsAllGuessed();
        }

        Boolean IsAllGuessed()
        {
            foreach (var row in guessedMatrix)
            {
                foreach (var col in row)
                {
                    if (!col)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
