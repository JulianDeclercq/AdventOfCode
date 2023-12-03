use serde::{Deserialize, Serialize};
use std::{fs, sync::Arc};

use reqwest::{blocking::Client, cookie::Jar, Url};

#[derive(Serialize, Deserialize, Debug)]
struct Config {
    session_token: String,
}

pub(crate) fn read_input(day: i32) -> Result<(), Box<dyn std::error::Error>> {
    read_input_for_year(day, 2023)
}

pub(crate) fn read_input_for_year(day: i32, year: i32) -> Result<(), Box<dyn std::error::Error>> {
    let file = fs::read_to_string("config.json").expect("failed to read config");
    let config: Config = serde_json::from_str(&file)?;
    let input_url = format!("https://adventofcode.com/{}/day/{}/input", year, day);

    let cookie = format!("session={}", config.session_token);
    let url = input_url.parse::<Url>()?;

    let jar = Jar::default();
    jar.add_cookie_str(cookie.as_str(), &url);
    let shared_jar = Arc::new(jar);

    let client = Client::builder()
        .cookie_store(true)
        .cookie_provider(shared_jar)
        .build()?;

    let input = client.get(input_url).send()?.text()?;

    println!("input: {}", input);
    Ok(())
}
