import './ContractDetails.css'
import React from 'react'
import FiveDots from '../../FiveDots'
import Tabs from '../../../core/Tabs'

const EmptyClause = {
	label: '',
	dots: 0,
	description: '',
	cost: '',
	dicePool: '',
	action: '',
	catch: '',
	resultDramatic: '',
	resultFailure: '',
	resultSuccess: '',
	resultExceptional: '',
	modifiers: []
}

export default class ContractDetails extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			editMode: false
		}
	}
	
	render()
	{
		let clauses = []
		
		if(this.props.contract !== null)
		{
			console.log({
				before: this.props.contract.clauses
			})
			this.props.contract.clauses.forEach((clause, i) => {
				clauses.push(
					this.state.editMode
						? this.generateEditView(clause, i)
						: this.generateDisplayView(clause, i)
				)
			})
		}
		
		if(clauses.length < this.props.contract.dots)
		{
			for(let i = 0; i < this.props.contract.dots - clauses.length; i++)
			{
				let newClause = Object.assign({}, EmptyClause)
				clauses.push(
					this.state.editMode
						? this.generateEditView(newClause, i)
						: this.generateDisplayView(newClause, i)
				)
			}
		}
		
		return (
			<Tabs className="fullscreenOverlayItem">
				{clauses}
			</Tabs>
		)
	}
	
	generateDisplayView(clause, key)
	{
		let {mods, sits} = this.generateModifierLists(clause)
		
		return (
			<div className={`contractDetails ${this.props.className}`} key={key}>
				<div className="row">
					{clause.label}
					<FiveDots value={clause.dots} valueChangedHandler={() => {}} />
					<button onClick={() => this.toggleEditMode()}>Edit</button>
				</div>
				<hr />
				<div className="row">{clause.description}</div>
				<hr />
				<div className="row indent">Cost: {clause.cost}</div>
				<div className="row indent">Dice Pool: {clause.dicePool}</div>
				<div className="row indent">Action: {clause.action}</div>
				<div className="row indent">Catch: {clause.catch}</div>
				<hr />
				<div className="column">
					<div className="row heading">Roll Results</div>
					<div className="row indent">Dramatic Failure: {clause.resultDramatic}</div>
					<hr />
					<div className="row indent">Failure: {clause.resultFailure}</div>
					<hr />
					<div className="row indent">Success: {clause.resultSuccess}</div>
					<hr />
					<div className="row indent">Exceptional Success: {clause.resultExceptional}</div>
				</div>
				<hr />
				<div className="row">Suggested Modifiers</div>
				<hr />
				<div className="row indent">
					<div className="column">
						<div>Modifier</div>
						{mods}
					</div>
					<div className="column">
						<div>Situation</div>
						{sits}
					</div>
				</div>
			</div>
		)
	}
	
	generateEditView(clause, key)
	{
		let mods = this.generateModifierLists(clause)
		
		return (
			<div className={`contractDetails ${this.props.className}`} key={key}>
				<div className="row">
					<input type="text" value={clause.label} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'label', e.target.value)} />
					<FiveDots value={clause.dots} valueChangedHandler={(arg) => this.props.changeHandler(this.props.contract, clause, 'dots', arg)} />
					<button onClick={() => this.toggleEditMode()}>Done</button>
				</div>
				<hr />
				<div className="row">
					<textarea value={clause.description} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'description', e.target.value)} />
				</div>
				<hr />
				<div className="row indent spacer">
					Cost:
					<input type="text" value={clause.cost} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'cost', e.target.value)} />
				</div>
				<div className="row indent spacer">
					Dice Pool:
					<input type="text" value={clause.dicePool} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'dicePool', e.target.value)} />
				</div>
				<div className="row indent spacer">
					Action:
					<input type="text" value={clause.action} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'action', e.target.value)} />
				</div>
				<div className="column indent">
					Catch:
					<textarea value={clause.catch} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'catch', e.target.value)} />
				</div>
				<hr />
				<div className="column">
					<div className="row heading">Roll Results</div>
					<div className="column indent">
						Dramatic Failure:
						<textarea value={clause.resultDramatic} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultDramatic', e.target.value)} />
					</div>
					<hr />
					<div className="column indent">
						Failure:
						<textarea value={clause.resultFailure} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultFailure', e.target.value)} />
					</div>
					<hr />
					<div className="column indent">
						Success:
						<textarea value={clause.resultSuccess} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultSuccess', e.target.value)} />
					</div>
					<hr />
					<div className="column indent">
						Exceptional Success:
						<textarea value={clause.resultExceptional} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'resultExceptional', e.target.value)} />
					</div>
				</div>
				<hr />
				<div className="row">Suggested Modifiers</div>
				<hr />
				<div className="column indent modifiers">
					<div className="row label">
						<div className="small">Modifier</div>
						<div>Situation</div>
					</div>
					{mods}
				</div>
			</div>
		)
	}
	
	generateModifierLists(clause)
	{
		let out = null
		let mode = this.state.editMode
		
		if(mode)
		{
			out = []
			clause.modifiers.forEach((obj, i) => {
				out.push(
					(<div className="row" key={`modifierRow-${i}`}>
						<input type="text" className="small spacer" value={obj.modifier} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'modifier', e.target.value, i)} />
						<textarea className="spacer" value={obj.situation} onChange={(e) => this.props.changeHandler(this.props.contract, clause, 'situation', e.target.value, i)} />
					</div>)
				)
			})
		}
		else
		{
			out = {
				mods: [],
				sits: []
			}
			
			clause.modifiers.forEach((obj, i) => {
				out.mods.push(
					(<div key={`modifier-${i}`}>{obj.modifier}</div>)
				)
				out.sits.push(
					(<div key={`situation-${i}`}>{obj.situation}</div>)
				)
			})
		}
		
		return out
	}
	
	toggleEditMode()
	{
		let mode = this.state.editMode
		this.setState(() => { return { editMode: !mode } })
	}
}
