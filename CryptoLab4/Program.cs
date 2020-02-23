using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace CryptoLab4
{

	class Register {

		public int lenght; 
		int[] index;

//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public Register(int polynomial, int lenght) {

			this.lenght = lenght; 

			int ones = 0;

			for (int i = 0; i < 7; i++) ones += ((1 << i) & polynomial) >> i ;

			this.index = new int[ones];

			int t = -1;
			for (int i = 0; i<7; i++) if (((1 << i) & polynomial) != 0) this.index[t+=1] = i;
		}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		public int[] RunRegisterRun (int start, int N) {

			int[] seq = new int[N];

			for (int i = 0; i < lenght; i++) {
				seq[i] = ((1 << i) & start) >> i;
			}

			for (int i = 0; i < N - lenght; i++) {
				for (int j = 0; j < index.Length; j++) {
					seq[i + lenght] ^= seq[i + index[j]];
				}
			}

			return seq;
		}
	}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

	class Program
	{

		static string z_str = "00111100011010100111011000000101011111001011010000101001001111010011101111110010011010000011011110100001011001001000010010111010001110000100010100110111100011010110100111100111001110000010111011111111010110010110011010101110110111011010000101000010010000010111010100010111010010001101010001010111000010111100011110100011100011011010010011001001011100001101100100110000011110101100100001000100000101011111111101010101110011001000111001110110110001110001000010101001011010010101011011110000111101101000111001111101111011010101100110001001010000010010001100100001110001010100010011011111100111011111010110000001111000010000111110101111010111111011101001011010000100001010101101000100010011101011111111101011000000001110011011101100110110001010010011001101100110100011011000111000111110100110100010101010100011111010111010110101100011000000010000010111000101100001010111001000000100010100101011000100110110101100111010110111001110001001001111010011011110010101011100100110110111110111110111010110100010110111010110101011101011100100010101011000100101111011110101010100101101010111011111100001101111010100011011011011111100010100110111100110111111011011101010011011111110100101001100111010101101000010111010010000111001110001000001110011010111111111101111111000110011001010011110001111100011110100110100001010001111000100110011000101000011100001000001110001001100011000010111111010001100101110011110001111101101101001010010011100101100111110010100101100110001111101011001101111001011001001001110000110000110000001001101101100110001000101010010001110000110010001000001000110101000011100100101101011101101111101000100110000100101000110001101010011100101011110001111000100111110010111010000111101010001100101011111100110110011110001110000100101110101110011100110111101011100111101001000101000001100100001101110000101101011011001010101100110110110111000111011101111001110010010000010111010001111000000011001010010100000110001100000010101000111110101000100001100100101000011000010100100001110110011101101111010001100100101001011011111001110001011010010110011";

		static int[] z = new int[2048];
		static ulong[] Z;

		static Register L1 = new Register(0b1010011, 30);
		static Register L2 = new Register(0b1001, 31);
		static Register L3 = new Register(0b10101111, 32);

//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		static void Main(string[] args)
		{

			for (int i = 0; i < z_str.Length; i++) z[i] = Convert.ToInt32(z_str.Substring(i, 1), 2);

			//FindCadidate();
			//FindL3();
			//Z = ToUlong(z);
			//ulong[] temp = Geffe(L1.RunRegisterRun(5133948, 2048), L2.RunRegisterRun(3832581, 2048), L3.RunRegisterRun(14449981, 2048));

			int[] temp = L3.RunRegisterRun(1 << (L3.lenght - 1), 2048);

			for (int i = 0; i < 2048; i++) {

				//Console.WriteLine(temp[i] ^ Z[i]);
				Console.Write(temp[i]);
			}

			//Console.WriteLine("End");
			Console.ReadKey();
		}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		static void FindCadidate () {

			int[] N = new int[] { 229, 236 };

			int[] C = new int[] { 60, 75 };

			List<int> Kandidats1 = new List<int>();
			List<int> Kandidats2 = new List<int>();

			int[] mass1 = new int[4];
			int[] mass2 = new int[4];

			int f = (1 << L1.lenght) / 4; 

			for (int i = 0; i < 3; i++) mass1[i] = f + f*i;

			mass1[3] = (1 << L1.lenght);

			f = (1 << L2.lenght) / 4;

			for (int i = 0; i < 3; i++) mass2[i] = f + f * i;

			mass2[3] = (1 << L2.lenght);

			Parallel.Invoke(

				() => {

					for (int i = 0; i < mass1[0]; i++) {

						int[] x = L1.RunRegisterRun(i, N[0]);

						int R = 0;

						for (int j = 0; j < N[0]; j++) R += x[j] ^ z[j];

						if (R < C[0]) {

							Kandidats1.Add(i);
							Console.WriteLine(R);
						}
					}
				},

				() => {

					for (int i = mass1[0]; i < mass1[1]; i++) {

						int[] x = L1.RunRegisterRun(i, N[0]);

						int R = 0;

						for (int j = 0; j < N[0]; j++) R += x[j] ^ z[j];

						if (R < C[0]) {

							Kandidats1.Add(i);
						}
					}
				},

				() => {

					for (int i = mass1[1]; i < mass1[2]; i++) {

						int[] x = L1.RunRegisterRun(i, N[0]);

						int R = 0;

						for (int j = 0; j < N[0]; j++) R += x[j] ^ z[j];

						if (R < C[0]) {

							Kandidats1.Add(i);
						}
					}
				},

				() => {

					for (int i = mass1[2]; i < mass1[3]; i++) {

						int[] x = L1.RunRegisterRun(i, N[0]);

						int R = 0;

						for (int j = 0; j < N[0]; j++) R += x[j] ^ z[j];

						if (R < C[0]) {

							Kandidats1.Add(i);
						}
					}
				},

				() => {

					for (int i = 0; i < mass2[0]; i++) {

						int[] y = L2.RunRegisterRun(i, N[1]);

						int R = 0;

						for (int j = 0; j < N[1]; j++) R += y[j] ^ z[j];

						if (R < C[1]) {

							Kandidats2.Add(i);
						}
					}
				},

				() => {

					for (int i = mass2[0]; i < mass2[1]; i++) {

						int[] y = L2.RunRegisterRun(i, N[1]);

						int R = 0;

						for (int j = 0; j < N[1]; j++) R += y[j] ^ z[j];

						if (R < C[1]) {

							Kandidats2.Add(i);
						}
					}
				},

				() => {

					for (int i = mass2[1]; i < mass2[2]; i++) {

						int[] y = L2.RunRegisterRun(i, N[1]);

						int R = 0;

						for (int j = 0; j < N[1]; j++) R += y[j] ^ z[j];

						if (R < C[1]) {

							Kandidats2.Add(i);
						}
					}
				},

				() => {

					for (int i = mass2[2]; i < mass2[3]; i++) {

						int[] y = L2.RunRegisterRun(i, N[1]);

						int R = 0;

						for (int j = 0; j < N[1]; j++) R += y[j] ^ z[j];

						if (R < C[1]) {

							Kandidats2.Add(i);
						}
					}
				}
			);

			string[] k = new string[2];

			k[0] = "";
			k[1] = "";

			foreach (int a in Kandidats1) k[0] += a + "\n";
			foreach (int a in Kandidats2) k[1] += a + "\n";

			StreamWriter wr = new StreamWriter(@"C:\Users\PRIDE\source\repos\CryptoLab4\kandidats1.txt");

			wr.Write(k[0]);
			wr.Close();

			wr = new StreamWriter(@"C:\Users\PRIDE\source\repos\CryptoLab4\kandidats2.txt");

			wr.Write(k[1]);
			wr.Close();
		}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		static void FindL3 () {

			List<int> Kandidats1 = new List<int>();
			List<int> Kandidats2 = new List<int>();
			List<int> Kandidats3 = new List<int>();

			StreamReader sr = new StreamReader(@"C:\Users\PRIDE\source\repos\CryptoLab4\kandidats1.txt");

			string curLine = "";
			while ((curLine = sr.ReadLine()) != null) Kandidats1.Add(Convert.ToInt32(curLine));
			sr.Close();

			sr = new StreamReader(@"C:\Users\PRIDE\source\repos\CryptoLab4\kandidats2.txt");

			curLine = "";
			while ((curLine = sr.ReadLine()) != null) Kandidats2.Add(Convert.ToInt32(curLine));
			sr.Close();

			int[] y = L2.RunRegisterRun(3832581, 2048);

			int f = (1 << L3.lenght)/6; 

			int[] mass = new int[6];

			for (int i = 0; i < 5; i++) mass[i] = f + i*f;
			mass[5] = 1 << L3.lenght;

			foreach (int i in Kandidats1) {

				int[] x = L1.RunRegisterRun(i, 2048);

				Parallel.Invoke(

					() => {

						for (int k = 0; k < mass[0]; k++) {

							if (L3Can(x, y, k) == 0) {

								Console.WriteLine("L1: " + i);
								Console.WriteLine("L2: " + 3832581);
								Console.WriteLine("L3: " + k);
								return;
							}
						}
					},

					() => {

						for (int k = mass[0]; k < mass[1]; k++) {

							if (L3Can(x, y, k) == 0) {

								Console.WriteLine("L1: " + i);
								Console.WriteLine("L2: " + 3832581);
								Console.WriteLine("L3: " + k);
								return;
							}
						}
					},

					() => {

						for (int k = mass[1]; k < mass[2]; k++) {

							if (L3Can(x, y, k) == 0) {

								Console.WriteLine("L1: " + i);
								Console.WriteLine("L2: " + 3832581);
								Console.WriteLine("L3: " + k);
								return;
							}
						}
					},

					() => {

						for (int k = mass[2]; k < mass[3]; k++) {

							if (L3Can(x, y, k) == 0) {

								Console.WriteLine("L1: " + i);
								Console.WriteLine("L2: " + 3832581);
								Console.WriteLine("L3: " + k);
								return;
							}
						}
					},

					() => {

						for (int k = mass[3]; k < mass[4]; k++) {

							if (L3Can(x, y, k) == 0) {

								Console.WriteLine("L1: " + i);
								Console.WriteLine("L2: " + 3832581);
								Console.WriteLine("L3: " + k);
								return;
							}
						}
					},

					() => {

						for (int k = mass[4]; k < mass[5]; k++) {

							if (L3Can(x, y, k) == 0) {

								Console.WriteLine("L1: " + i);
								Console.WriteLine("L2: " + 3832581);
								Console.WriteLine("L3: " + k);
								return;
							}
						}
					}
				);

			}
		}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		static ulong[] ToUlong (int[] x) {

			ulong[] result = new ulong[32];

			for (int i = 0; i < 32; i++) for (int j = 0; j<64; j++) result[i] |= (ulong) (x[j + 64 * i]) << (63 - j);
				
			return result;
		}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|

		static ulong[] Geffe (int[] x, int[] y, int[] s) {

			ulong[] result = new ulong[32];

			for (int i = 0; i < 32; i++) for (int j = 0; j < 64; j++) result[i] |= (ulong) ((s[j + 64 * i] & x[j + 64 * i]) ^ ((1 ^ s[j + 64 * i]) & y[j + 64 * i])) << (63 - j);

			return result;
		}

//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
		static int L3Can (int[] x, int[] y, int k) {

			int[] s = L3.RunRegisterRun(k, 2048);

			int t = 0;
			for (int l = 0; l < 2048; l++) {

				t = ((s[l] & x[l]) ^ ((1 ^ s[l]) & y[l])) ^ z[l];
				if (t != 0) return -1;
			}

			return t;
		}
	}
}
