mod input;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    input::read_input_for_year(3, 2023)?;
    Ok(())
}
