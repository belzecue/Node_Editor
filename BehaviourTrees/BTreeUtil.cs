using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Reflection;
using System;
using BehaviourTrees;

public class BTreeUtil  {
	/// <summary>
	/// 指定したクラスの全てのメソッドを取得。
	/// </summary>
	/// <returns>The methods.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static string[] GetMethods<T>(){
		//Tを継承したクラスを全て取得
		var clacces = Assembly.GetAssembly(typeof(T)).GetTypes().Where(t => t.IsSubclassOf(typeof(T)));
		return clacces
			.SelectMany(c=>c.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			.Select(p=>p.Name).ToArray();
		
	}

	/// <summary>
	/// 指定のGameObjectの指定のメソッドのDelegateを取得
	/// </summary>
	/// <returns>The action.</returns>
	/// <param name="go">Go.</param>
	/// <param name="type">Type.</param>
	/// <param name="methodName">Method name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static Action<T> GetAction<T>(GameObject go, Type type, string methodName){
		var clacces = Assembly.GetAssembly(type).GetTypes().Where(t => t.IsSubclassOf(type));
		var query = clacces
			.SelectMany(c=>c.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

		Component comp = null;
		foreach(var c in clacces){
			///Publicメソッドを取得(継承されたメンバは含まない)
			foreach (var m in c.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				if(m.Name == methodName){
					comp =  go.GetComponent(c.Name);
				}
			}
		}
		var mquery = query.Where(p=>p.Name == methodName);
		var minfo = mquery.FirstOrDefault();
		var result =  (Action<T>)Delegate.CreateDelegate(typeof(Action<T>),comp,minfo);
		return result;
	}


	/// <summary>
	/// 指定のGameObjectの指定のメソッドのDelegateを取得
	/// </summary>
	/// <returns>The func.</returns>
	/// <param name="go">Go.</param>
	/// <param name="type">Type.</param>
	/// <param name="methodName">Method name.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	/// <typeparam name="T1">The 2nd type parameter.</typeparam>
	public static Func<T,T1> GetFunc<T,T1>(GameObject go ,Type type, string methodName){
		var clacces = Assembly.GetAssembly(type).GetTypes().Where(t => t.IsSubclassOf(type));
		Component comp = null;
		foreach(var c in clacces){
			///Publicメソッドを取得(継承されたメンバは含まない)
			foreach (var m in c.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				if(m.Name == methodName){
					comp =  go.GetComponent(c.Name);
				}
			}
		}
		var query = clacces
			.SelectMany(c=>c.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
		var mquery = query.Where(p=>p.Name == methodName);
		var minfo = mquery.FirstOrDefault();
		var result =  (Func<T,T1>)Delegate.CreateDelegate(typeof(Func<T,T1>),comp,minfo);
		return result;
	}

}
