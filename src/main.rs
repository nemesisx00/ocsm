use web_view::*;

fn main()
{
	let html = "<html><body><h1>Hello</h1></body></html>";

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
