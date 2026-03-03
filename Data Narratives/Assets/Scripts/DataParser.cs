using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataParser : MonoBehaviour
{
    public static DataParser Instance; //singleton
    public TextAsset dataset; //take the dataset and parse it to choose five specific countries
    public Dictionary<string,float> countries = new Dictionary<string,float>();
    List<string> target_countries = new List<string>{"Hungary","Peru","Jordan","India","Somalia"}; //filter out five countries


    void Awake()
    {
        if (Instance == null) {Instance = this;}
        LoadData();
    }

    void LoadData()
    {
        string[] lines = dataset.text.Split('\n');

        //go through the dataset by line
        for (int i = 1; i < lines.Length; i++) { //exclude header
            string[] columns = lines[i].Split(';'); //splitting each item by ;
            string country_name = columns[0].Trim(); //trimming names

            if (target_countries.Contains(country_name)) {
                //change ghi_2025 into float after parsing special characters
                float ghi_score = ParseGHI(columns[5]);
                countries[country_name] = ghi_score;
            }
        }
    }

    float ParseGHI(string string_ghi) {
        if (string_ghi.Contains("<5")) {return 4.9f;} //change to float for comparison
        if (string_ghi == "-" || string_ghi == "*") {return 0.0f;} //missing data

        float result;
        if (float.TryParse(string_ghi.Replace(',','.'), out result)) {return result;} //this in case the project is opened on different countries where systems ta

        return 0f;
    }
}
