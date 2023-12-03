mod input;

fn main() -> Result<(), Box<dyn std::error::Error>> {
    input::read_input_for_year(6, 2015)?;
    Ok(())
}
