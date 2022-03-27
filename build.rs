#![allow(non_snake_case)]

use std::{
	io::{
		Write,
		stdout,
		stderr
	},
	process::Command
};

/// Generate the unified and compressed app.css for the OCSM application
/// using the Stylus CSS Preprocessor.
fn main()
{
	let mut program = "cmd";
	let mut firstArg = "/C";
	
	if !cfg!(target_os = "windows")
	{
		program = "sh";
		firstArg = "-c";
	}
	
	let output = Command::new(program)
		.args(&[firstArg, "npm run-script stylus"])
		.output()
		.expect("Failed to execute Stylus script");
	
	println!("cargo:rerun-if-changed=static/app.css");
	println!("cargo:rerun-if-changed=stylus/**/*");
}
