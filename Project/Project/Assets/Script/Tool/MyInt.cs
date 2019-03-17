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
        MyInt ret = new MyInt();

        return ret;
    }

    public static MyInt operator +(MyInt lhs, MyInt rhs)
    {
        MyInt ret = new MyInt();
        int indexlhs = lhs.IntValue.Length - 1;
        int indexrhs = rhs.IntValue.Length - 1;
        bool isAdd = false;
        ret.IntValue = "";
        do
        {
            int i = (isAdd ? Convert.ToInt32(ret.IntValue[0]) : 0)
                + Convert.ToInt32(lhs.IntValue[indexlhs].ToString())
                + Convert.ToInt32(rhs.IntValue[indexrhs].ToString());
            if (i > 10)
            {
                isAdd = true;
            }

            ret.IntValue = i.ToString() + ret.IntValue;
            indexlhs--;
            indexrhs--;

        } while (indexlhs >= 0 && indexrhs >= 0);
        if (indexlhs >= 0)
        {
            for (int i = indexlhs; i >= 0; i--)
            {
                ret.IntValue = lhs.IntValue[i].ToString() + ret.IntValue;
            }
        }
        else if (indexrhs >= 0)
        {
            for (int i = indexrhs; i >= 0; i--)
            {
                ret.IntValue = rhs.IntValue[i].ToString() + ret.IntValue;
            }
        }
        return ret;
    }

    public static bool operator ==(MyInt lhs, MyInt rhs)
    {
        return lhs.IntValue == rhs.IntValue;
    }

    public static bool operator !=(MyInt lhs, MyInt rhs)
    {
        return lhs.IntValue != rhs.IntValue;
    }

    public static bool operator <=(MyInt lhs, MyInt rhs)
    {
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
        return base.ToString();
    }

    public override bool Equals(object obj)
    {
        return IntValue == ((MyInt)obj).IntValue;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
