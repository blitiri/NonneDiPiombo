using System;

/// <summary>
/// Test utilities.
/// </summary>
public class TestUtilities
{
	/// <summary>
	/// Produce a dummy ranking.
	/// </summary>
	/// <returns>The dummy ranking.</returns>
	/// <param name="debugTestCase">Test case to generate.</param>
	public static RankingPosition[] GetDummyRanking (int debugTestCase)
	{
		RankingPosition[] ranking;

		switch (debugTestCase) {
		case 1:
			// Two players
			ranking = new RankingPosition[2];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, -5, "MadamWheelchairPlayerIcon");
			break;
		case 2:
			// Two players with same score
			ranking = new RankingPosition[2];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 10, "MadamWheelchairPlayerIcon");
			break;
		case 3:
			// Three players
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, -5, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 7, "MsShakyPlayerIcon");
			break;
		case 4:
			// Three players with two with same score
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 7, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 10, "MsShakyPlayerIcon");
			break;
		case 5:
			// Three players with two with same score
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (0, 7, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 10, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 7, "MsShakyPlayerIcon");
			break;
		case 6:
			// Three players with same score
			ranking = new RankingPosition[3];
			ranking [0] = new RankingPosition (0, 7, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 7, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 7, "MsShakyPlayerIcon");
			break;
		case 7:
			// Four players with two with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 10, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 7, "MsShakyPlayerIcon");
			ranking [3] = new RankingPosition (3, 0, "CatLadyPlayerIcon");
			break;
		case 8:
			// Four players with three with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 10, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 10, "MsShakyPlayerIcon");
			ranking [3] = new RankingPosition (3, 0, "CatLadyPlayerIcon");
			break;
		case 9:
			// Four players with three with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 10, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 10, "MsShakyPlayerIcon");
			ranking [3] = new RankingPosition (3, 22, "CatLadyPlayerIcon");
			break;
		case 10:
			// Four players with same score
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, 10, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 10, "MsShakyPlayerIcon");
			ranking [3] = new RankingPosition (3, 10, "CatLadyPlayerIcon");
			break;
		case 11:
		default:
			// Four players
			ranking = new RankingPosition[4];
			ranking [0] = new RankingPosition (0, 10, "ChefAgataPlayerIcon");
			ranking [1] = new RankingPosition (1, -5, "MadamWheelchairPlayerIcon");
			ranking [2] = new RankingPosition (2, 7, "MsShakyPlayerIcon");
			ranking [3] = new RankingPosition (3, 0, "CatLadyPlayerIcon");
			break;
		}
		Array.Sort (ranking);
		return ranking;
	}
}
