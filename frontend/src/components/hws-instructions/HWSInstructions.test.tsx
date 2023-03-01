import { render, screen } from '@testing-library/react';
import HWSInstructions from './HWSInstructions';

test('renders the instructions alert text', () => {
  render(<HWSInstructions />);
  const instructionsElement = screen.getByText(/Todo List App/i);
  expect(instructionsElement).toBeInTheDocument();
});
