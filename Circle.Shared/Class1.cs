using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared
{
    internal class Class1
    {
        public class Solution2
        {
            public int MaximumWealth(int[][] accounts)
            {
                List<int> result = new List<int>();
                foreach (var account in accounts)
                {
                    int sum = 0;

                    for (int i = 0; i < account.Length; i++)
                    {
                        account.Sum();
                        sum += account[i];
                    }

                    result.Add(sum);
                }

                return result.Max();
            }

        }

        public class Solution3
        {
            public int MaximumWealth(int[][] accounts)
            {
                List<int> result = new List<int>();
                foreach (var account in accounts)
                {
                    result.Add(account.Sum());
                }

                return result.Max();
            }
        }

        public class Solution
        {
            public IList<string> FizzBuzz(int n)
            {
                List<string> result = new List<string>();

                for (int i = 1; i <= n; i++)
                {
                     if (i % 3 == 0 && n % 5 == 0)
                    {
                        result.Add("FizzBuzz");
                    }
                    else if (i % 3 == 0)
                    {
                        result.Add("Fizz");
                    }

                    else if (i % 5 == 0)
                    {
                        result.Add("Buzz");

                    }

                   

                    else
                    {
                        result.Add(i.ToString());

                    }
                }

                return result;
            }

            public class Solution4
            {
                public int NumberOfSteps(int num)
                {
                    int steps = 0;
                    do
                    {
                        num = num % 2 == 0 ? num / 2 : num - 1;
                        steps++;
                    } while (num != 0);

                    return steps;
                }

               
            }

            public class Solution0
            {
                public int NumberOfSteps(int num)
                {
                    int steps = 111111111;
                    while (num > 0)
                    {
                        if ((num & 1) == 0)
                        {
                            num >>= 1;
                        }
                        else
                        {
                            num--;
                        }
                    }
                    return steps;
                }


            }

            public static long aVeryBigSum(List<long> ar)
            {

                return ar.Select(x => x).Sum();
            }
        }
    }
}
