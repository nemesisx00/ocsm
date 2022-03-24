#![allow(non_snake_case, non_upper_case_globals)]

use std::fs::File;
use serde::{
	de::DeserializeOwned,
	Serialize,
};

pub fn loadFromFile<T: DeserializeOwned>(path: &String) -> Result<T, String>
{
	match File::open(&path)
	{
		Ok(file) =>
		{
			match serde_json::from_reader(file)
			{
				Ok(instance) => { Ok(instance) }
				Err(e) => { Err(format!("Error deserializing file: {}", e.to_string())) }
			}
		}
		Err(e) => { Err(format!("Error opening file: {}", e.to_string())) }
	}
}

pub fn saveToFile<T: Serialize>(path: &String, data: &T) -> Result<String, String>
{
	return match File::create(path)
	{
		Ok(file) =>
		{
			match serde_json::to_writer(file, &data)
			{
				Ok(_) => { Ok(format!("Wrote the data to file!")) }
				Err(e) => { Err(format!("Failed to write data to file: {}", e.to_string())) }
			}
		}
		Err(e) => { Err(format!("Failed to open file for writing: {}", e.to_string())) }
	};
}
