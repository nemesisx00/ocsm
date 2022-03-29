#![allow(non_snake_case, non_upper_case_globals)]

use std::{
	fs::File,
	path::Path,
};
use directories::UserDirs;
use native_dialog::FileDialog;
use serde::{
	de::DeserializeOwned,
	Serialize,
};

pub fn getFilePath(save: bool, currentPath: Option<String>) -> Option<String>
{
	let initialLocation = match currentPath
	{
		Some(p) => p,
		None => getUserDocumentsDir()
	};
	
	let dialog = FileDialog::new()
		.set_location(&initialLocation)
		.add_filter("JSON", &["json"])
		.add_filter("All Files", &["*"]);
	
	let result = match save
	{
		true => dialog.show_save_single_file(),
		false => dialog.show_open_single_file()
	};
	
	return match result
	{
		Ok(pbo) => match pbo
		{
			Some(pb) => Some(pb.into_os_string().into_string().unwrap()),
			None => None,
		},
		Err(_) => None,
	};
}

fn getUserDocumentsDir() -> String
{
	return match UserDirs::new()
	{
		Some(dirs) => dirs.document_dir().unwrap().to_str().unwrap().to_string(),
		None => "~/Documents".to_string(),
	};
}

pub fn loadFromFile<T: DeserializeOwned>(path: &Path) -> Result<T, String>
{
	return match File::open(&path)
	{
		Ok(file) => match serde_json::from_reader(file)
		{
			Ok(instance) => Ok(instance),
			Err(e) => Err(format!("Error deserializing file: {}", e.to_string()))
		},
		Err(e) => Err(format!("Error opening file: {}", e.to_string()))
	};
}

pub fn saveToFile<T: Serialize>(path: &Path, data: &T) -> Result<String, String>
{
	return match File::create(path)
	{
		Ok(file) => match serde_json::to_writer(file, &data)
		{
			Ok(_) => Ok(format!("Wrote the data to file!")),
			Err(e) => Err(format!("Failed to write data to file: {}", e.to_string()))
		},
		Err(e) => Err(format!("Failed to open file for writing: {}", e.to_string()))
	};
}
