use serde::{Deserialize, Serialize};
use std::{fs, sync::Arc};

use reqwest::{blocking::Client, cookie::Jar, Url};

#[derive(Serialize, Deserialize, Debug)]
struct Config {
    session_token: String,
}

fn main() -> reqwest::Result<()> {
    let file = fs::read_to_string("config.json").expect("failed to read config");
    let config: Config = serde_json::from_str(&file).unwrap();
    let input_url = "https://adventofcode.com/2015/day/6/input";

    let cookie = format!("session={}", config.session_token);
    let url = input_url.parse::<Url>().unwrap();

    let jar = Jar::default();
    jar.add_cookie_str(cookie.as_str(), &url);
    let shared_jar = Arc::new(jar);

    let client = Client::builder()
        .cookie_store(true)
        .cookie_provider(shared_jar)
        .build()?;

    let input = client.get(input_url).send()?.text()?;

    println!("input: {:?}", input);
    Ok(())
}
