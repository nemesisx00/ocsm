
fn main()
{
	glib_build_tools::compile_resources(
		&["templates"],
		"templates/templates.gresource.xml",
		"templates.gresource"
	);
}
