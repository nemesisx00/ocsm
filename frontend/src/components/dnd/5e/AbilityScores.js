import './AbilityScores.css'
import React from 'react'
import {normalizeClassNames} from '../../core/Utilities'

const Abilities = Object.freeze({
	Strength: 'strength',
	Dexterity: 'dexterity',
	Constitution: 'constitution',
	Intelligence: 'intelligence',
	Wisdom: 'wisdom',
	Charisma: 'charisma'
})

const DownPath = 'M0,0 L6,12 L12,0 Z'
const UpPath = 'M0,12 L6,0 L12,12 Z'
const ViewBox = '0 0 12 12'

const ScoreMax = 30
const ScoreMin = 1

const CalculateAbilityModifier = score => {
	let num = NormalizeAbilityScore(score)
	if(Number.isInteger(num))
	{
		let mod = Math.trunc(num / 2) - 5
		return mod >= 0 ? `+${mod}` : mod
	}
	return 'N/A'
}

const NormalizeAbilityScore = score => {
	let num = Number.parseInt(score)
	if(Number.isInteger(num))
	{
		if(num > ScoreMax)
			num = ScoreMax
		if(num < ScoreMin)
			num = ScoreMin
		return num
	}
	return score
}

class AbilityScore extends React.Component
{
	render()
	{
		return (
			<div className={normalizeClassNames('abilityScore', this.props.className)}>
				<div className="scoreLabel">{this.props.label}</div>
				<div className="scoreModifier">{CalculateAbilityModifier(this.props.value)}</div>
				<div className="scoreValue row">
					<input type="text" value={this.props.value} onChange={(e) => this.props.changeHandler(NormalizeAbilityScore(e.target.value))} />
					<div className="buttons column">
						<div className="button up" onClick={(e) => this.props.changeHandler(NormalizeAbilityScore(this.props.value + 1))}>
							<svg xmlns="http://www.w3.org/2000/svg" viewBox={ViewBox}>
								<path d={UpPath} />
							</svg>
						</div>
						<div className="button down" onClick={(e) => this.props.changeHandler(NormalizeAbilityScore(this.props.value - 1))}>
							<svg xmlns="http://www.w3.org/2000/svg" viewBox={ViewBox}>
								<path d={DownPath} />
							</svg>
						</div>
					</div>
				</div>
			</div>
		)
	}
}

class AbilityScores extends React.Component
{
	render()
	{
		return (
			<div className="abilityScores">
				<AbilityScore className="strength" label="Strength" value={this.props.abilityScores.strength} changeHandler={(score) => this.props.changeHandler(Abilities.Strength, score)} />
				<AbilityScore className="dexterity" label="Dexterity" value={this.props.abilityScores.dexterity} changeHandler={(score) => this.props.changeHandler(Abilities.Dexterity, score)} />
				<AbilityScore className="constitution" label="Constitution" value={this.props.abilityScores.constitution} changeHandler={(score) => this.props.changeHandler(Abilities.Constitution, score)} />
				<AbilityScore className="intelligence" label="Intelligence" value={this.props.abilityScores.intelligence} changeHandler={(score) => this.props.changeHandler(Abilities.Intelligence, score)} />
				<AbilityScore className="wisdom" label="Wisdom" value={this.props.abilityScores.wisdom} changeHandler={(score) => this.props.changeHandler(Abilities.Wisdom, score)} />
				<AbilityScore className="charisma" label="Charisma" value={this.props.abilityScores.charisma} changeHandler={(score) => this.props.changeHandler(Abilities.Charisma, score)} />
			</div>
		)
	}
}

AbilityScores.Abilities = Abilities

export default AbilityScores
