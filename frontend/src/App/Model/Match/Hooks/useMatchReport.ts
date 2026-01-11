import type { Match } from "../Entities/Match";

const apiErrorMessage =
  "An error occurred. Please try again or contact support if it persists";

export const useMatchReport = () => {
  const fetchShowcaseMatch = async (): Promise<Match> => {
    try {
      const response = await fetch(`http://localhost:5065/Showcase/get`);

      if (!response.ok) {
        alert(apiErrorMessage);
      }

      return response.json();
    } catch (error) {
      alert(apiErrorMessage);
      throw error;
    }
  };

  return { fetchShowcaseMatch };
};
