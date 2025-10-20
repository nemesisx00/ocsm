use std::fs::File;
use std::io::{BufReader, BufWriter, Read, Write};
use std::path::{Path, PathBuf};
use std::sync::LazyLock;
use anyhow::{Context, Result};
use bytes::Bytes;
use directories::BaseDirs;
use rfd::AsyncFileDialog;

const AssetPath: LazyLock<PathBuf> = LazyLock::new(|| {
	return Path::new(env!("CARGO_MANIFEST_DIR"))
		.join("assets");
});

pub const CircleEmpty: &str = "circle-empty.png";
pub const CircleFull: &str = "circle-fill.png";
pub const CircleHalf: &str = "circle-fill-half.png";
pub const CircleRed: &str = "circle-fill-red.png";
pub const BoxBorder: &str = "box-border-16.png";
#[allow(unused)]
pub const BoxEmpty: &str = "box-transparent-16.png";
pub const SlashOne: &str = "slash-one.png";
pub const SlashTwo: &str = "slash-two.png";
pub const SlashThree: &str = "slash-three.png";

pub async fn chooseLoadFile() -> Option<PathBuf>
{
	let dirs = BaseDirs::new()?;
	
	let file = AsyncFileDialog::new()
		.add_filter("ocsd", &["ocsd"])
		.add_filter("json", &["json"])
		.set_directory(dirs.home_dir())
		.pick_file()
		.await?;
	
	return Some(file.path().to_owned());
}

pub async fn chooseSaveFile() -> Option<PathBuf>
{
	let dirs = BaseDirs::new()?;
	
	let file = AsyncFileDialog::new()
		.add_filter("ocsd", &["ocsd"])
		.add_filter("json", &["json"])
		.set_directory(dirs.home_dir())
		.save_file()
		.await?;
	
	return Some(file.path().to_owned());
}

pub fn loadFromFile(path: PathBuf) -> Result<String>
{
	let file = File::open(&path)
		.context(format!("Failed to open file for reading at: {:?}", path))?;
	let mut reader = BufReader::new(file);
	
	let mut json = String::default();
	reader.read_to_string(&mut json)?;
	
	return Ok(json);
}

pub fn loadImageToBytes(assetName: &String) -> Result<Bytes>
{
	let path = AssetPath.join(assetName);
	let file = File::open(path)
		.context("Failed to open image file for reading")?;
	
	let mut reader = BufReader::new(file);
	let mut buffer = vec![];
	reader.read_to_end(&mut buffer)
		.context("Failed to read image file to end")?;
	
	return Ok(Bytes::from_owner(buffer));
}

pub fn saveToFile(json: String, path: PathBuf) -> Result<()>
{
	let file = File::create(&path)
		.context(format!("Failed to open file for writing at: {:?}", path))?;
	let mut writer = BufWriter::new(file);
	_ = writer.write_all(json.as_bytes())
		.context(format!("Failed to write json to file at: {:?}", path))?;
	return Ok(());
}

/*
let mut reader = BufReader::new(file);
	let mut json = String::default();
	_ = reader.read_to_string(&mut json);
	
	this.obj().emit_by_name::<()>(
		super::HomeScreen::Signal_LoadSheet,
		&[&json]
	);
}
*/