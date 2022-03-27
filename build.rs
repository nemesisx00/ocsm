#![allow(non_snake_case)]

use std::process::Command;

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
	
	Command::new(program)
		.args(&[firstArg, "npm run-script stylus"])
		.output()
		.expect("Failed to execute Stylus script");
	
	//Just always re-run this script
	println!("cargo:rerun-if-changed=static/app.css");
}
