use std::io;

fn main() {
    println!("type something..");
    let mut input = String::new();
    io::stdin()
        .read_line(&mut input)
        .expect("Failed to read input");

    println!("user typed {}", input);
}
