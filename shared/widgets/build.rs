
fn main()
{
	glib_build_tools::compile_resources(
		&["templates"],
		"templates/widgets-templates.gresource.xml",
		"templates.gresource"
	);
}
