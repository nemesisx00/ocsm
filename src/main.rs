use web_view::*;

fn main()
{
	let html = include_str!("../html/main.html");

	web_view::builder()
		.title("Open Character Sheet Manager")
		.content(Content::Html(html))
		.size(640, 480)
		.resizable(true)
		.debug(true)
		.user_data(())
		.invoke_handler(|_webview, _arg| Ok(()))
		.run()
		.unwrap();
}
