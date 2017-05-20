using System;

/// <summary>
/// Test utilities.
/// </summary>
public class TestUtilities
{
	/// <summary>
	/// The chef Agata player icon.
	/// </summary>
	private static readonly string chefAgata = "ChefAgataPlayerIcon";
	/// <summary>
	/// The madame Ho player icon.
	/// </summary>
	private static readonly string madameHo = "MadameHoPlayerIcon";
	/// <summary>
	/// The Ms. Shaky player icon.
	/// </summary>
	private static readonly string msShaky = "MsShakyPlayerIcon";
	/// <summary>
	/// The Cat lady player icon.
	/// </summary>
	private static readonly string catLady = "CatLadyPlayerIcon";

	/// <summary>
	/// Produce a dummy ranking.
	/// </summary>
	/// <returns>The dummy ranking.</returns>
	/// <param name="debugTestCase">Test case to generate.</param>
	public static RankingPosition[] GetDummyRanking (int debugTestCase)
	{
		RankingPosition[] ranking;
		int playerIndex;

		playerIndex = 0;
		switch (debugTestCase) {
		case 1:
			// Two players
			ranking = new RankingPosition[2];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, -5, madameHo);
			break;
		case 2:
			// Two players with same score
			ranking = new RankingPosition[2];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, madameHo);
			break;
		case 3:
			// Three players
			ranking = new RankingPosition[3];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, -5, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, msShaky);
			break;
		case 4:
			// Three players with two with same score
			ranking = new RankingPosition[3];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, msShaky);
			break;
		case 5:
			// Three players with two with same score
			ranking = new RankingPosition[3];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, msShaky);
			break;
		case 6:
			// Three players with same score
			ranking = new RankingPosition[3];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, msShaky);
			break;
		case 7:
			// Four players with two with same score
			ranking = new RankingPosition[4];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, msShaky);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 0, catLady);
			break;
		case 8:
			// Four players with three with same score
			ranking = new RankingPosition[4];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, msShaky);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 0, catLady);
			break;
		case 9:
			// Four players with three with same score
			ranking = new RankingPosition[4];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, msShaky);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 22, catLady);
			break;
		case 10:
			// Four players with same score
			ranking = new RankingPosition[4];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, msShaky);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, catLady);
			break;
		case 11:
		default:
			// Four players
			ranking = new RankingPosition[4];
			ranking [playerIndex++] = new RankingPosition (playerIndex, 10, chefAgata);
			ranking [playerIndex++] = new RankingPosition (playerIndex, -5, madameHo);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 7, msShaky);
			ranking [playerIndex++] = new RankingPosition (playerIndex, 0, catLady);
			break;
		}
		Array.Sort (ranking);
		return ranking;
	}
}
