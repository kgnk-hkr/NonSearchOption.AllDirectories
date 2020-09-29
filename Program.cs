using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace TextSearcherNoMethod
{
    class Items
    {
        public string file { get; set; }
        public string create_time { get; set; }
    }

    class Texts
    {
        List<Items> texts = new List<Items>();

        public void AddItem(string getfile, string getcreate_time)
        {
            try
            {
                texts.Add(new Items() { file = getfile, create_time = getcreate_time });
            }
            catch
            {
                Console.WriteLine("リストへの追加に失敗しました");
            }
        }
    }

    class Program
    {
        static void scan(string dir)
        {
            string[] subdirs = Directory.GetFiles(dir, "*.txt");//dirの中のtxtを探す

            try
            {
                foreach (string subdir in subdirs)
                {
                    //作成日を取得するための処理
                    var date = new FileInfo(subdir);

                    //取得した情報をAddItemに渡す
                    Texts txts = new Texts();
                    txts.AddItem(subdir, date.CreationTime.ToString());

                    //一時表示
                    // JSON化前にリスト化する
                    List<Items> texts = new List<Items>
                        {
                            new Items{ file = subdir , create_time = date.CreationTime.ToString()}
                        };
                    //JSON化
                    string json = JsonSerializer.Serialize(texts);
                    Console.WriteLine(json);

                }

                //ファイルを探し終わったらディレクトリを探して、subdirに入れる。はじめのscan(args[0]);と同じ処理ルートになる
                foreach (string subdir in Directory.GetDirectories(dir))
                {
                    scan(subdir);
                }
            }
            catch
            {
                Console.WriteLine("探索中にエラーが発生しました。");
            }

        }

        static int Main(string[] args)
        {
            // 引数チェック
            //引数の数が正しく入力されていることの確認 
            if (args.Length != 1 && args.Length != 0)
            {
                Console.Write("引数はひとつにしてください\n");
                return -1;
            }
            //引数が入力されていることの確認
            else if (args.Length == 0)
            {
                Console.Write("引数に対象のフォルダパスを入力してください\n");
                return -1;
            }

            //fainally用
            StreamReader reader = null;

            try
            {
                //再帰
                scan(args[0]);

            }
            catch (DirectoryNotFoundException)
            {
                // エラー処理1
                // ディレクトリが存在しなかったり、アクセス権限がない場合にここが実行される。
                Console.Write("\nディレクトリが存在しない、もしくはアクセス権がありません");
                return -1;
            }
            catch (Exception)
            {
                // エラー処理2
                // ファイルが存在しなかったり、アクセス権限がない場合にここが実行される。
                Console.Write("\n該当のファイルが存在しない、もしくはアクセス権がありません");
                return -1;
            }
            finally
            {
                // 例外の有無にかかわらず終了する
                if (reader != null)
                    reader.Close();
            }

            return 0;

        }
    }
}
