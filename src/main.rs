use std::sync::Arc;

use reqwest::{blocking::Client, cookie::Jar, Url};

fn main() -> reqwest::Result<()> {
    let input_url = "https://adventofcode.com/2015/day/6/input";

    let cookie = "session=SESSION_COOKIE";
    let url = input_url.parse::<Url>().unwrap();

    let jar = Jar::default();
    jar.add_cookie_str(cookie, &url);
    let shared_jar = Arc::new(jar);

    let client = Client::builder()
        .cookie_store(true)
        .cookie_provider(shared_jar)
        .build()?;

    let input = client.get(input_url).send()?.text()?;
    println!("input: {:?}", input);

    Ok(())
}
