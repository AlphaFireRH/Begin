using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInt
{
    public MyInt()
    {
        IntValue = "0";
    }

    public string IntValue { get; set; }

    public static MyInt operator -(MyInt lhs, MyInt rhs)
    {
        if (lhs == null)
        {
            lhs = new MyInt();
        }
        if (rhs == null)
        {
            rhs = new MyInt();
        }

        MyInt ret = new MyInt();

        return ret;
    }

    public static MyInt operator +(MyInt lhs, MyInt rhs)
    {
        if (lhs == null)
        {
            lhs = new MyInt();
        }
        if (rhs == null)
        {
            rhs = new MyInt();
        }

        MyInt ret = new MyInt();
        int indexlhs = lhs.IntValue.Length - 1;
        int indexrhs = rhs.IntValue.Length - 1;
        bool isAdd = false;
        ret.IntValue = "";
        do
        {
            string temp = "0";
            if (isAdd)
            {
                temp = ret.IntValue[0].ToString();
                ret.IntValue = ret.IntValue.Remove(0, 1);
            }

            int i = (isAdd ? Convert.ToInt32(temp) : 0)
                + Convert.ToInt32(lhs.IntValue[indexlhs].ToString())
                + Convert.ToInt32(rhs.IntValue[indexrhs].ToString());
            if (i >= 10)
            {
                isAdd = true;
            }
            else
            {
                isAdd = false;
            }

            ret.IntValue = i.ToString() + ret.IntValue;

            indexlhs--;
            indexrhs--;

        } while (indexlhs >= 0 && indexrhs >= 0);
        if (indexlhs >= 0)
        {
            for (int i = indexlhs; i >= 0; i--)
            {
                if (isAdd)
                {
                    var temp = ret.IntValue[0].ToString();
                    ret.IntValue = ret.IntValue.Remove(0, 1);

                    temp = (Convert.ToInt32(lhs.IntValue[i].ToString()) + Convert.ToInt32(temp)).ToString();
                    ret.IntValue = temp + ret.IntValue;
                    if (Convert.ToInt32(temp) >= 10)
                    {
                        isAdd = true;
                    }
                    else
                    {
                        isAdd = false;
                    }
                }
                else
                {
                    ret.IntValue = lhs.IntValue[i].ToString() + ret.IntValue;
                }
            }
        }
        else if (indexrhs >= 0)
        {
            for (int i = indexrhs; i >= 0; i--)
            {
                if (isAdd)
                {
                    //var temp = rhs.IntValue[0].ToString();
                    //rhs.IntValue = ret.IntValue.Remove(0, 1);
                    //isAdd = false;
                    //ret.IntValue = (Convert.ToInt32(rhs.IntValue[i].ToString()) + Convert.ToInt32(temp)).ToString() + rhs.IntValue;

                    var temp = ret.IntValue[0].ToString();
                    ret.IntValue = ret.IntValue.Remove(0, 1);

                    temp = (Convert.ToInt32(rhs.IntValue[i].ToString()) + Convert.ToInt32(temp)).ToString();
                    ret.IntValue = temp + ret.IntValue;
                    if (Convert.ToInt32(temp) >= 10)
                    {
                        isAdd = true;
                    }
                    else
                    {
                        isAdd = false;
                    }
                }
                else
                {
                    ret.IntValue = rhs.IntValue[i].ToString() + ret.IntValue;
                }
            }
        }
        return ret;
    }

    public static bool operator ==(MyInt lhs, MyInt rhs)
    {
        try
        {
            lhs.IntValue = lhs.IntValue + "";
        }
        catch (Exception)
        {
            lhs = new MyInt();
        }
        try
        {
            rhs.IntValue = lhs.IntValue + "";
        }
        catch (Exception)
        {
            rhs = new MyInt();
        }
        return lhs.IntValue == rhs.IntValue;
    }

    public static bool operator !=(MyInt lhs, MyInt rhs)
    {
        if (lhs == null)
        {
            lhs = new MyInt();
        }
        if (rhs == null)
        {
            rhs = new MyInt();
        }
        try
        {

            return lhs.IntValue != rhs.IntValue;
        }
        catch (Exception)
        {
        }
        return false;

    }

    public static bool operator <=(MyInt lhs, MyInt rhs)
    {
        if (lhs == null)
        {
            lhs = new MyInt();
        }
        if (rhs == null)
        {
            rhs = new MyInt();
        }

        bool status = false;
        if (lhs.IntValue.Length < rhs.IntValue.Length)
        {
            status = true;
        }
        else if (lhs.IntValue.Length == rhs.IntValue.Length)
        {
            for (int i = 0; i < lhs.IntValue.Length || i < rhs.IntValue.Length; i++)
            {
                if (Convert.ToInt32(lhs.IntValue[i].ToString()) < Convert.ToInt32(rhs.IntValue[i].ToString()))
                {
                    status = true;
                    break;
                }
                else if ((i == lhs.IntValue.Length - 1 || i == rhs.IntValue.Length - 1)
                    && Convert.ToInt32(lhs.IntValue[i].ToString()) <= Convert.ToInt32(rhs.IntValue[i].ToString()))
                {
                    status = true;
                    break;
                }
            }
        }
        return status;
    }

    public static bool operator <(MyInt lhs, MyInt rhs)
    {
        if (lhs == null)
        {
            lhs = new MyInt();
        }
        if (rhs == null)
        {
            rhs = new MyInt();
        }

        bool status = false;
        if (lhs.IntValue.Length < rhs.IntValue.Length)
        {
            status = true;
        }
        else if (lhs.IntValue.Length == rhs.IntValue.Length)
        {
            for (int i = 0; i < lhs.IntValue.Length || i < rhs.IntValue.Length; i++)
            {
                if (Convert.ToInt32(lhs.IntValue[i].ToString()) < Convert.ToInt32(rhs.IntValue[i].ToString()))
                {
                    status = true;
                    break;
                }
            }
        }
        return status;
    }

    public static bool operator >(MyInt lhs, MyInt rhs)
    {
        if (lhs == null)
        {
            lhs = new MyInt();
        }
        if (rhs == null)
        {
            rhs = new MyInt();
        }

        bool status = false;
        if (lhs.IntValue.Length > rhs.IntValue.Length)
        {
            status = true;
        }
        else if (lhs.IntValue.Length == rhs.IntValue.Length)
        {
            for (int i = 0; i < lhs.IntValue.Length || i < rhs.IntValue.Length; i++)
            {
                if (Convert.ToInt32(lhs.IntValue[i].ToString()) > Convert.ToInt32(rhs.IntValue[i].ToString()))
                {
                    status = true;
                    break;
                }
            }
        }
        return status;
    }

    public static bool operator >=(MyInt lhs, MyInt rhs)
    {
        if (lhs == null)
        {
            lhs = new MyInt();
        }
        if (rhs == null)
        {
            rhs = new MyInt();
        }
        bool status = false;
        if (lhs.IntValue.Length > rhs.IntValue.Length)
        {
            status = true;
        }
        else if (lhs.IntValue.Length == rhs.IntValue.Length)
        {
            for (int i = 0; i < lhs.IntValue.Length || i < rhs.IntValue.Length; i++)
            {
                if (Convert.ToInt32(lhs.IntValue[i].ToString()) > Convert.ToInt32(rhs.IntValue[i].ToString()))
                {
                    status = true;
                    break;
                }
                else if ((i == lhs.IntValue.Length - 1 || i == rhs.IntValue.Length - 1)
                    && Convert.ToInt32(lhs.IntValue[i].ToString()) >= Convert.ToInt32(rhs.IntValue[i].ToString()))
                {
                    status = true;
                    break;
                }
            }
        }
        return status;
    }

    public override string ToString()
    {
        return IntValue;
    }

    public override bool Equals(object obj)
    {
        return IntValue == ((MyInt)obj).IntValue;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


    #region 计算2的N次方

    #endregion
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ladder"></param>
    /// <returns></returns>
    public static MyInt Pow2(int ladder)
    {
        MyInt ret = new MyInt();
        ret.IntValue = "1";

        for (int i = 0; i < ladder; i++)
        {
            ret += ret;
        }
        return ret;
    }
}
